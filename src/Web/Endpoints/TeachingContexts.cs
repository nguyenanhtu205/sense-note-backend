using Application.TeachingContexts.Commands.CreateTeachingContext;
using Application.TeachingContexts.Queries.GetTeachingContextsByTeacherId;

namespace Web.Endpoints;

public class TeachingContexts : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetTeachingContextsByTeacherId)
            .RequireAuthorization()
            .RequireRateLimiting("get");

        groupBuilder.MapPost(CreateTeachingContext)
            .RequireAuthorization()
            .RequireRateLimiting("post");
    }

    [EndpointSummary("Get teaching contexts")]
    [EndpointDescription("Returns all teaching contexts for the current teacher.")]
    public static async Task<IResult> GetTeachingContextsByTeacherId(ISender sender,
        CancellationToken cancellationToken)
    {
        TeachingContextsVm vm = await sender.Send(new GetTeachingContextsByTeacherIdQuery(), cancellationToken);
        return Results.Ok(vm);
    }

    [EndpointSummary("Create teaching context")]
    [EndpointDescription(
        "Creates a new teaching context. EnvironmentalAsset fields: AssetType = type of asset generating impact " +
        "(e.g. Speaker, Window, Projector), X = X coordinate on classroom grid (0-based), Y = Y coordinate on " +
        "classroom grid (0-based), InfluenceRadius = radius of effect area, ImpactType = type of impact (e.g. Noise, " +
        "Light, Distraction).")]
    public static async Task<IResult> CreateTeachingContext(
        CreateTeachingContextCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        int newTeachingContextId = await sender.Send(command, cancellationToken);
        return Results.Ok(newTeachingContextId);
    }
}
