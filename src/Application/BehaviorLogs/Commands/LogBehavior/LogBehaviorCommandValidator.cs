namespace Application.BehaviorLogs.Commands.LogBehavior;

public class LogBehaviorCommandValidator : AbstractValidator<LogBehaviorCommand>
{
    public LogBehaviorCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("Lesson id must be greater than 0");

        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student id must be greater than 0");
    }
}
