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
    [EndpointDescription("Creates a new teaching context.")]
    public static async Task<IResult> CreateTeachingContext(
        CreateTeachingContextCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        int newTeachingContextId = await sender.Send(command, cancellationToken);
        return Results.Ok(newTeachingContextId);
    }
}
