namespace Application.Students.Commands.AddStudent;

public class AddStudentCommandValidator : AbstractValidator<AddStudentCommand>
{
    public AddStudentCommandValidator()
    {
        RuleFor(x => x.ClassId)
            .GreaterThan(0).WithMessage("Class id must be greater than 0");

        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(150).WithMessage("Full name must not exceed 150 characters");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .MaximumLength(100).WithMessage("Display name must not exceed 100 characters");

        RuleFor(x => x.OrdinalIndex).NotNull();

        RuleFor(x => x.BirthDay)
            .LessThan(DateTime.Today).When(x => x.BirthDay.HasValue)
            .WithMessage("Birthday must be in the past");
    }
}
