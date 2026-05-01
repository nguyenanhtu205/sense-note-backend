namespace Application.BehaviorLogs.Queries.GetStudentBehaviorLogHistory;

public record StudentBehaviorLogHistoryItemVm(
    string BehaviorCategoryName,
    int PointValue,
    DateTimeOffset OccurredAt,
    string Antecedent,
    string BehaviorDescription,
    string Consequence,
    int SeverityLevel
);

public class StudentBehaviorLogHistoryVm
{
    public IReadOnlyCollection<StudentBehaviorLogHistoryItemVm> Logs { get; init; } = [];
}
