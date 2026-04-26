namespace Application.BehaviorLogs.Queries.GetStudentBehaviorLogHistory;

public record StudentBehaviorLogHistoryItemVm(string BehaviorCategoryName, int PointValue, DateTimeOffset OccurredAt);

public class StudentBehaviorLogHistoryVm
{
    public IReadOnlyCollection<StudentBehaviorLogHistoryItemVm> Logs { get; init; } = [];
}
