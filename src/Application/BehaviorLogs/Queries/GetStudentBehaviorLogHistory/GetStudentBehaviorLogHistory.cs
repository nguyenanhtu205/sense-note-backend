namespace Application.BehaviorLogs.Queries.GetStudentBehaviorLogHistory;

public record GetStudentBehaviorLogHistoryQuery(int StudentId, int LessonId) : IRequest<StudentBehaviorLogHistoryVm>;

public class GetStudentBehaviorLogHistoryQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetStudentBehaviorLogHistoryQuery, StudentBehaviorLogHistoryVm>
{
    public async Task<StudentBehaviorLogHistoryVm> Handle(GetStudentBehaviorLogHistoryQuery request,
        CancellationToken cancellationToken)
    {
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
                new StudentBehaviorLogHistoryItemVm(x.BehaviorCategory!.Name, x.BehaviorCategory.PointValue,
                    x.OccurredAt)).ToList()
        };
    }
}
