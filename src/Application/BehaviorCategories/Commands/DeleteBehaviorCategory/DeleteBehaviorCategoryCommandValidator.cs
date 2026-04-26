namespace Application.BehaviorCategories.Commands.DeleteBehaviorCategory;

public class DeleteBehaviorCategoryCommandValidator : AbstractValidator<DeleteBehaviorCategoryCommand>
{
    public DeleteBehaviorCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Behavior category id must be greater than 0");
    }
}
