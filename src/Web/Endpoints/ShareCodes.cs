using Application.ShareCodes.Commands.CreateTeachingContextShareCode;
using Application.ShareCodes.Commands.ImportTeachingContextShareCode;

namespace Web.Endpoints;

public record CreateTeachingContextShareCodeResponse(string Code);

public record CreateTeachingContextByShareCodeResponse(int NewTeachingContextId);

public class ShareCodes : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateTeachingContextShareCode, "create")
            .Produces<CreateTeachingContextShareCodeResponse>()
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPost(CreateTeachingContextByShareCode, "import")
            .Produces<CreateTeachingContextByShareCodeResponse>()
            .RequireAuthorization()
            .RequireRateLimiting("post");
    }

    [EndpointSummary("Create share code")]
    [EndpointDescription("Creates a share code that allows other teachers to copy the class layout and student list.")]
    public static async Task<IResult> CreateTeachingContextShareCode(CreateTeachingContextShareCodeCommand command,
        ISender sender, CancellationToken cancellationToken)
    {
        string code = await sender.Send(command, cancellationToken);
        return Results.Ok(new CreateTeachingContextShareCodeResponse(code));
    }

    [EndpointSummary("Import share code")]
    [EndpointDescription("Imports a share code to copy the class layout and student list from another teacher.")]
    public static async Task<IResult> CreateTeachingContextByShareCode(ImportTeachingContextShareCodeCommand command,
        ISender sender, CancellationToken cancellationToken)
    {
        int newTeachingContextId = await sender.Send(command, cancellationToken);
        return Results.Ok(new CreateTeachingContextByShareCodeResponse(newTeachingContextId));
    }
}
