namespace Application.BehaviorLogs.Queries.GetClassBehaviorLogHistory;

public record ClassBehaviorLogHistoryItemVm(
    string DisplayName,
    string BehaviorCategoryName,
    int PointValue,
    DateTimeOffset OccurredAt);

public class ClassBehaviorLogHistoryVm
{
    public IReadOnlyCollection<ClassBehaviorLogHistoryItemVm> Logs { get; init; } = [];
}
