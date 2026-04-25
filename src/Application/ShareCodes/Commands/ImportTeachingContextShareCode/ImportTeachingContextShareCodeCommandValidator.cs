namespace Application.ShareCodes.Commands.ImportTeachingContextShareCode;

public class ImportTeachingContextShareCodeCommandValidator : AbstractValidator<ImportTeachingContextShareCodeCommand>
{
    public ImportTeachingContextShareCodeCommandValidator()
    {
        RuleFor(v => v.TeachingContextName)
            .MaximumLength(255).WithMessage("Teaching context name can't be more than 255 characters")
            .NotEmpty().WithMessage("Teaching context name is required");

        RuleFor(v => v.ShareCode)
            .NotEmpty().WithMessage("Class name is required");
    }
}
