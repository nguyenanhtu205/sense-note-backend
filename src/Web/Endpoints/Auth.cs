using Application.Auth.Commands.Register;

namespace Web.Endpoints;

public class Auth : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(Register, "register");
    }

    [EndpointSummary("Register")]
    [EndpointDescription("Create a new user account.")]
    public static async Task<IResult> Register(
        RegisterCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        RegisterResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }
}
