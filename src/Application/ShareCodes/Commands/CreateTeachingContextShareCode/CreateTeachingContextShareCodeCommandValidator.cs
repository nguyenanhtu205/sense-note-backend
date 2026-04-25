namespace Application.ShareCodes.Commands.CreateTeachingContextShareCode;

public class CreateTeachingContextShareCodeCommandValidator : AbstractValidator<CreateTeachingContextShareCodeCommand>
{
    public CreateTeachingContextShareCodeCommandValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0)
            .WithMessage("Teaching context id must be greater than 0");
    }
}
