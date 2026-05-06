namespace Application.AiServices.Commands.AnalyzeAbcTrends;

public class AnalyzeAbcTrendsCommandValidator : AbstractValidator<AnalyzeAbcTrendsCommand>
{
    public AnalyzeAbcTrendsCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("StudentId must be greater than 0.");

        RuleFor(x => x.LessonIds)
            .NotNull().WithMessage("Lesson ids cannot be null.")
            .NotEmpty().WithMessage("Lesson ids cannot be empty.")
            .Must(ids => ids.All(id => id > 0)).WithMessage("All lesson id must be positive integers.");
    }
}
