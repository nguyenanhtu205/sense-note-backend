namespace Application.AiServices.Commands.GetSuggestedIntervention;

public class GetSuggestedInterventionCommandValidator : AbstractValidator<GetSuggestedInterventionCommand>
{
    public GetSuggestedInterventionCommandValidator()
    {
        RuleFor(x => x.BehaviorLogId)
            .GreaterThan(0).WithMessage("Behavior log id must be greater than 0");
    }
}
