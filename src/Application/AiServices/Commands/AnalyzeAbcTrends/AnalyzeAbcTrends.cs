namespace Application.AiServices.Commands.AnalyzeAbcTrends;

public record AnalyzeAbcTrendsCommand(int StudentId, List<int> LessonIds) : IRequest<AnalyzeAbcTrendsResponse>;

public class AnalyzeAbcTrendsCommandHandler(IApplicationDbContext context, IAnalyzeAbcTrends analyzeAbcTrends)
    : IRequestHandler<AnalyzeAbcTrendsCommand, AnalyzeAbcTrendsResponse>
{
    public async Task<AnalyzeAbcTrendsResponse> Handle(AnalyzeAbcTrendsCommand request,
        CancellationToken cancellationToken)
    {
        List<BehaviorLog> behaviorLogs = await context.BehaviorLogs
            .Where(bl => bl.StudentId == request.StudentId && request.LessonIds.Contains(bl.LessonId))
            .OrderBy(bl => bl.OccurredAt)
            .ToListAsync(cancellationToken);

        return behaviorLogs.Count switch
        {
            0 => throw new NotFoundException("No behavior logs found for the given student and lessons."),
            < 5 => new AnalyzeAbcTrendsResponse
            {
                TrendSummary = "Not enough data to analyze trends.",
                RecommendedIntervention = "Log more behavior for better analysis."
            },
            _ => await analyzeAbcTrends.Analyze(
                new AnalyzeAbcTrendsRequest
                {
                    AbcTrends = behaviorLogs.Select(bl => new AbcTrend
                    {
                        Antecedent = bl.Antecedent,
                        BehaviorDescription = bl.BehaviorDescription,
                        Consequence = bl.Consequence,
                        SeverityLevel = bl.SeverityLevel
                    }).ToList(),
                    StartTime = behaviorLogs.First().OccurredAt.ToString("o"),
                    EndTime = behaviorLogs.Last().OccurredAt.ToString("o")
                }, cancellationToken) ?? throw new AiServiceException()
        };
    }
}
