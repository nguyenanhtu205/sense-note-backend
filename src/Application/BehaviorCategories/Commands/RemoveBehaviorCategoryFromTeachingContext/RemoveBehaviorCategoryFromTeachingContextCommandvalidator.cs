namespace Application.BehaviorCategories.Commands.RemoveBehaviorCategoryFromTeachingContext;

public class
    RemoveBehaviorCategoryFromTeachingContextCommandValidator : AbstractValidator<
    RemoveBehaviorCategoryFromTeachingContextCommand>
{
    public RemoveBehaviorCategoryFromTeachingContextCommandValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");

        RuleForEach(x => x.BehaviorCategoryId)
            .GreaterThan(0).WithMessage("Behavior category id must be greater than 0");
    }
}
