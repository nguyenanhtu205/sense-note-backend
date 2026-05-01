namespace Application.BehaviorLogs.Queries.GetStudentBehaviorLogHistory;

public record GetStudentBehaviorLogHistoryQuery(int StudentId, int LessonId) : IRequest<StudentBehaviorLogHistoryVm>;

public class GetStudentBehaviorLogHistoryQueryHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<GetStudentBehaviorLogHistoryQuery, StudentBehaviorLogHistoryVm>
{
    public async Task<StudentBehaviorLogHistoryVm> Handle(GetStudentBehaviorLogHistoryQuery request,
        CancellationToken cancellationToken)
    {
        Lesson? lesson = await context.Lessons
            .Where(l => l.Id == request.LessonId)
            .Include(l => l.TeachingContext)
            .FirstOrDefaultAsync(cancellationToken);

        if (lesson == null)
        {
            throw new NotFoundException($"Lesson with id {request.LessonId} was not found");
        }

        if (lesson.TeachingContext!.TeacherId != int.Parse(currentTeacher.Id!))
        {
            throw new ForbiddenAccessException("You don't have permission to access this resources");
        }

        List<BehaviorLog> logs = await context.BehaviorLogs
            .AsNoTracking()
            .Where(x => x.StudentId == request.StudentId && x.LessonId == request.LessonId)
            .Include(x => x.BehaviorCategory)
            .ToListAsync(cancellationToken);

        if (logs.Count == 0)
        {
            return new StudentBehaviorLogHistoryVm { Logs = [] };
        }

        return new StudentBehaviorLogHistoryVm
        {
            Logs = logs.Select(x =>
                new StudentBehaviorLogHistoryItemVm(
                    x.BehaviorCategory!.Name,
                    x.BehaviorCategory.PointValue,
                    x.OccurredAt,
                    x.Antecedent,
                    x.BehaviorDescription,
                    x.Consequence,
                    x.SeverityLevel
                )).ToList()
        };
    }
}
