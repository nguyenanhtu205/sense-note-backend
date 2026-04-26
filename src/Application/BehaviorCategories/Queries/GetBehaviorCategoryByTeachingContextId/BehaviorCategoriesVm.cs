namespace Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeachingContextId;

public class BehaviorCategoriesVm
{
    public IReadOnlyCollection<BehaviorCategoryDto> BehaviorCategories { get; init; } = [];
}
