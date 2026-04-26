namespace Application.BehaviorCategories.Commands.CreateBehaviorCategory;

public class CreateBehaviorCategoryCommandValidator : AbstractValidator<CreateBehaviorCategoryCommand>
{
    public CreateBehaviorCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Behavior category name can't be more than 100 characters")
            .NotEmpty().WithMessage("Behavior category name name is required");
    }
}
