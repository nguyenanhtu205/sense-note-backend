namespace Application.Lessons.Commands.EndLesson;

public class EndLessonCommandValidator : AbstractValidator<EndLessonCommand>
{
    public EndLessonCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0)
            .WithMessage("Lesson id must be greater than 0");
    }
}
