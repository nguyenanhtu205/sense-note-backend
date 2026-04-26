namespace Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeacherId;

public class BehaviorCategoriesVm
{
    public IReadOnlyCollection<BehaviorCategoryDto> BehaviorCategories { get; init; } = [];
}
