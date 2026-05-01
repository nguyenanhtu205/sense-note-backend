namespace Application.BehaviorLogs.Queries.GetClassBehaviorLogHistory;

public record ClassBehaviorLogHistoryItemVm(
    string DisplayName,
    string BehaviorCategoryName,
    int PointValue,
    DateTimeOffset OccurredAt,
    string Antecedent,
    string BehaviorDescription,
    string Consequence,
    int SeverityLevel
);

public class ClassBehaviorLogHistoryVm
{
    public IReadOnlyCollection<ClassBehaviorLogHistoryItemVm> Logs { get; init; } = [];
}
