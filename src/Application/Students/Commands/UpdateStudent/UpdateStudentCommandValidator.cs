namespace Application.Students.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(v => v.StudentId)
            .GreaterThan(0).WithMessage("Student id must be greater than 0");

        RuleFor(v => v.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(150).WithMessage("Full name must not exceed 150 characters");

        RuleFor(x => x.BirthDay)
            .LessThan(DateTime.Today).When(x => x.BirthDay.HasValue)
            .WithMessage("Birthday must be in the past");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .MaximumLength(100).WithMessage("Display name must not exceed 100 characters");
        
        RuleFor(x => x.StudentSensitivityProfile)
            .NotNull().WithMessage("Student sensitivity profile must not be null");
    }
}
