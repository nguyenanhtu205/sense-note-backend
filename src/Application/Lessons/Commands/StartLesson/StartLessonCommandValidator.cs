namespace Application.Lessons.Commands.StartLesson;

public class StartLessonCommandValidator : AbstractValidator<StartLessonCommand>
{
    public StartLessonCommandValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0)
            .WithMessage("Teaching context id must be greater than 0");

        RuleFor(x => x.LessonName)
            .MaximumLength(150).WithMessage("Lesson name can't be more than 150 characters")
            .NotEmpty().WithMessage("Lesson name is required");
    }
}
