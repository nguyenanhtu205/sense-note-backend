namespace Application.AiServices.Commands.ExtractScores;

public class ExtractScoresCommandValidator : AbstractValidator<ExtractScoresCommand>
{
    public ExtractScoresCommandValidator()
    {
        RuleFor(x => x.MedicalNote)
            .NotNull().WithMessage("Medical note is required.")
            .NotEmpty().WithMessage("Medical note cannot be empty.")
            .MaximumLength(5000).WithMessage("Medical note cannot exceed 5000 characters.");
    }
}
