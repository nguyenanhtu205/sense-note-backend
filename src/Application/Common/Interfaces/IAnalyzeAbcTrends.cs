namespace Application.Common.Interfaces;

public record AbcTrend
{
    public required string Antecedent { get; init; }
    public required string BehaviorDescription { get; init; }
    public required string Consequence { get; init; }
    public int SeverityLevel { get; init; }
}

public record AnalyzeAbcTrendsRequest
{
    public List<AbcTrend> AbcTrends { get; init; } = [];
    public required string StartTime { get; init; }
    public required string EndTime { get; init; }
}

public record AnalyzeAbcTrendsResponse
{
    public required string TrendSummary { get; init; }
    public required string RecommendedIntervention { get; init; }
}

public interface IAnalyzeAbcTrends
{
    Task<AnalyzeAbcTrendsResponse?> Analyze(AnalyzeAbcTrendsRequest request, CancellationToken cancellationToken);
}
