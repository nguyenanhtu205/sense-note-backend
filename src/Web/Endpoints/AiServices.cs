using Application.AiServices.Commands.AnalyzeAbcTrends;
using Application.AiServices.Commands.ExtractScores;
using Application.AiServices.Commands.GetSuggestedIntervention;
using Application.Common.Interfaces;

namespace Web.Endpoints;

public class AiServices : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost("/extract-scores", ExtractScores)
            .Produces<MedicalSensitivityScoresResponse>()
            .Produces(StatusCodes.Status502BadGateway)
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPost("/get-suggested-intervention", GetSuggestedIntervention)
            .Produces<GetSuggestedInterventionResponse>()
            .Produces(StatusCodes.Status502BadGateway)
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPost("/analyze-abc-trends", AnalyzeAbcTrends)
            .Produces<AnalyzeAbcTrendsResponse>()
            .Produces(StatusCodes.Status502BadGateway)
            .RequireAuthorization()
            .RequireRateLimiting("post");
    }

    [EndpointSummary("Extract scores")]
    [EndpointDescription("""
                         Extracts Sound sensitivity, Light sensitivity, Temperature sensitivity, Touch sensitivity, 
                         Distractibility from a given Medical Note using AI services.
                         """)]
    public static async Task<IResult> ExtractScores(ExtractScoresCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        MedicalSensitivityScoresResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get suggested intervention")]
    [EndpointDescription("Gets a suggested intervention for a given behavior log using AI services.")]
    public static async Task<IResult> GetSuggestedIntervention(GetSuggestedInterventionCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        GetSuggestedInterventionResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Analyze ABC trends")]
    [EndpointDescription("""
                         Analyzes ABC trends for a given student and lessons using AI services.
                         Requires at least 5 behavior logs to provide a meaningful analysis.
                         """)]
    public static async Task<IResult> AnalyzeAbcTrends(AnalyzeAbcTrendsCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        AnalyzeAbcTrendsResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }
}
