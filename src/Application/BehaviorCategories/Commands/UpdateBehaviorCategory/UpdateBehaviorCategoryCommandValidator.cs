namespace Application.BehaviorCategories.Commands.UpdateBehaviorCategory;

public class UpdateBehaviorCategoryCommandValidator : AbstractValidator<UpdateBehaviorCategoryCommand>
{
    public UpdateBehaviorCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Behavior category name can't be more than 100 characters")
            .NotEmpty().WithMessage("Behavior category name name is required");

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Behavior category id must be greater than 0");
    }
}
