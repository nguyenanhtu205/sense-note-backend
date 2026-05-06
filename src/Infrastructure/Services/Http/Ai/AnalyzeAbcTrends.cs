namespace Infrastructure.Services.Http.Ai;

public class AnalyzeAbcTrends(HttpClient httpClient) : AiHttpClientBase(httpClient), IAnalyzeAbcTrends
{
    public async Task<AnalyzeAbcTrendsResponse?> Analyze(AnalyzeAbcTrendsRequest request,
        CancellationToken cancellationToken)
    {
        return await PostAsync<object, AnalyzeAbcTrendsResponse>(
            "/api/v1/llm_request/abc/analyze",
            new
            {
                records = request.AbcTrends.Select(t => new
                {
                    antecedent = t.Antecedent,
                    behavior_description = t.BehaviorDescription,
                    consequence = t.Consequence,
                    severity_level = t.SeverityLevel
                }).ToList(),
                start_time = request.StartTime,
                end_time = request.EndTime
            },
            cancellationToken);
    }
}
