namespace Application.Common.Interfaces;

public record GetSuggestedInterventionRequest
{
    public required string Antecedent { get; init; }
    public required string BehaviorDescription { get; init; }
    public required string Consequence { get; init; }
    public int SeverityLevel { get; init; }
}

public record GetSuggestedInterventionResponse
{
    public required string SuggestedIntervention { get; init; }
}

public interface IGetSuggestedIntervention
{
    Task<GetSuggestedInterventionResponse?> GetSuggestedInterventionAsync(GetSuggestedInterventionRequest request,
        CancellationToken cancellationToken);
}
