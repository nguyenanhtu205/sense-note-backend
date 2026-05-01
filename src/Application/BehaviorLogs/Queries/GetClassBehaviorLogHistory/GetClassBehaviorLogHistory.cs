namespace Application.BehaviorLogs.Queries.GetClassBehaviorLogHistory;

public record GetClassBehaviorLogHistoryQuery(int TeachingContextId, int LessonId)
    : IRequest<ClassBehaviorLogHistoryVm>;

public class GetClassBehaviorLogHistoryQueryHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<GetClassBehaviorLogHistoryQuery, ClassBehaviorLogHistoryVm>
{
    public async Task<ClassBehaviorLogHistoryVm> Handle(GetClassBehaviorLogHistoryQuery request,
        CancellationToken cancellationToken)
    {
        TeachingContext? teachingContext = await context.TeachingContexts
            .AsNoTracking()
            .Include(x => x.SeatAssignments)
            .FirstOrDefaultAsync(tc => tc.Id == request.TeachingContextId, cancellationToken);

        if (teachingContext == null)
        {
            throw new NotFoundException($"Teaching context with id {request.TeachingContextId} was not found");
        }

        if (teachingContext.TeacherId != int.Parse(currentTeacher.Id!))
        {
            throw new ForbiddenAccessException("You don't have permission to access this resource");
        }

        ICollection<SeatAssignment> seatAssignments = teachingContext.SeatAssignments;

        Dictionary<int, string> studentDisplayNames = seatAssignments
            .ToDictionary(sa => sa.StudentId, sa => sa.DisplayName);

        List<int> studentIds = seatAssignments.Select(sa => sa.StudentId).ToList();

        List<BehaviorLog> classLogs = await context.BehaviorLogs
            .AsNoTracking()
            .Where(bl => bl.LessonId == request.LessonId && studentIds.Contains(bl.StudentId))
            .Include(bl => bl.BehaviorCategory)
            .ToListAsync(cancellationToken);

        return new ClassBehaviorLogHistoryVm
        {
            Logs = classLogs.Select(log => new ClassBehaviorLogHistoryItemVm(
                studentDisplayNames.GetValueOrDefault(log.StudentId) ?? "Unknown",
                log.BehaviorCategory?.Name ?? "Unknown",
                log.BehaviorCategory?.PointValue ?? 0,
                log.OccurredAt,
                log.Antecedent,
                log.BehaviorDescription,
                log.Consequence,
                log.SeverityLevel
            )).ToList()
        };
    }
}
