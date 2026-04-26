namespace Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeachingContextId;

public class GetBehaviorCategoryByTeachingContextIdQueryValidator
    : AbstractValidator<GetBehaviorCategoriesByTeachingContextIdQuery>
{
    public GetBehaviorCategoryByTeachingContextIdQueryValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");
    }
}
