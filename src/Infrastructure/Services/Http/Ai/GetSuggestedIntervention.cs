namespace Infrastructure.Services.Http.Ai;

public class GetSuggestedIntervention(HttpClient httpClient) : AiHttpClientBase(httpClient), IGetSuggestedIntervention
{
    public async Task<GetSuggestedInterventionResponse?> GetSuggestedInterventionAsync(
        GetSuggestedInterventionRequest request, CancellationToken cancellationToken)
    {
        return await PostAsync<object, GetSuggestedInterventionResponse>(
            "/api/v1/llm_request/abc/suggested-intervention",
            new
            {
                antecedent = request.Antecedent,
                behavior_description = request.BehaviorDescription,
                consequence = request.Consequence,
                severity_level = request.SeverityLevel
            },
            cancellationToken);
    }
}
