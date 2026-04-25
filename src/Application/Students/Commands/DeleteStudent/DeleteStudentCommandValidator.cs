namespace Application.Students.Commands.DeleteStudent;

public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
{
    public DeleteStudentCommandValidator()
    {
        RuleFor(v => v.StudentId)
            .GreaterThan(0).WithMessage("Student id must be greater than 0");
    }
}
