namespace Application.BehaviorLogs.Commands.LogBehavior;

public class LogBehaviorCommandValidator : AbstractValidator<LogBehaviorCommand>
{
    public LogBehaviorCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("Lesson id must be greater than 0");

        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student id must be greater than 0");

        RuleFor(x => x.BehaviorCategoryId)
            .GreaterThan(0).WithMessage("Behavior category id must be greater than 0");

        RuleFor(x => x.Antecedent)
            .NotNull().WithMessage("Antecedent is required")
            .NotEmpty().WithMessage("Antecedent is required")
            .MaximumLength(500).WithMessage("Antecedent must not exceed 500 characters");

        RuleFor(x => x.BehaviorDescription)
            .NotNull().WithMessage("Behavior description is required")
            .NotEmpty().WithMessage("Behavior description is required")
            .MaximumLength(500).WithMessage("Behavior description must not exceed 500 characters");

        RuleFor(x => x.Consequence)
            .NotNull().WithMessage("Consequence is required")
            .NotEmpty().WithMessage("Consequence is required")
            .MaximumLength(500).WithMessage("Consequence must not exceed 500 characters");

        RuleFor(x => x.SeverityLevel)
            .InclusiveBetween(1, 5).WithMessage("Severity level must be between 1 and 5");
    }
}
