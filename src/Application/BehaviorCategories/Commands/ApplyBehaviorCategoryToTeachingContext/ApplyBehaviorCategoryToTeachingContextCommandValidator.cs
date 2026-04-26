namespace Application.BehaviorCategories.Commands.ApplyBehaviorCategoryToTeachingContext;

public class
    ApplyBehaviorCategoryToTeachingContextCommandValidator : AbstractValidator<
    ApplyBehaviorCategoryToTeachingContextCommand>
{
    public ApplyBehaviorCategoryToTeachingContextCommandValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");

        RuleForEach(x => x.BehaviorCategoryId)
            .GreaterThan(0).WithMessage("Behavior category id must be greater than 0");
    }
}
