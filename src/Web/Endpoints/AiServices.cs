using Application.AiServices.Commands.ExtractScores;
using Application.Common.Interfaces;

namespace Web.Endpoints;

public class AiServices : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost("/extract-scores", ExtractScores)
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
}
