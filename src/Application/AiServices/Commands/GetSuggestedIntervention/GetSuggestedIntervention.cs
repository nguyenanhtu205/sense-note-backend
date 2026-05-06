namespace Application.AiServices.Commands.GetSuggestedIntervention;

public record GetSuggestedInterventionCommand(int BehaviorLogId) : IRequest<GetSuggestedInterventionResponse>;

public class GetSuggestedInterventionCommandHandler(
    IApplicationDbContext context,
    IGetSuggestedIntervention getSuggestedIntervention)
    : IRequestHandler<GetSuggestedInterventionCommand, GetSuggestedInterventionResponse>
{
    public async Task<GetSuggestedInterventionResponse> Handle(GetSuggestedInterventionCommand request,
        CancellationToken cancellationToken)
    {
        BehaviorLog? behaviorLog =
            await context.BehaviorLogs.FindAsync([request.BehaviorLogId], cancellationToken);

        if (behaviorLog == null)
        {
            throw new NotFoundException($"Behavior log with id {request.BehaviorLogId} not found");
        }

        return await getSuggestedIntervention.GetSuggestedInterventionAsync(
            new GetSuggestedInterventionRequest
            {
                Antecedent = behaviorLog.Antecedent,
                BehaviorDescription = behaviorLog.BehaviorDescription,
                Consequence = behaviorLog.Consequence,
                SeverityLevel = behaviorLog.SeverityLevel
            },
            cancellationToken) ?? throw new AiServiceException();
    }
}
