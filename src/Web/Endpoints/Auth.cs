using Application.Auth.Commands.Login;
using Application.Auth.Commands.Logout;
using Application.Auth.Commands.RefreshAccessToken;
using Application.Auth.Commands.Register;

namespace Web.Endpoints;

public class Auth : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(Register, "register");
        groupBuilder.MapPost(Login, "login");
        groupBuilder.MapPost(Logout, "logout").RequireAuthorization();
        groupBuilder.MapPost(RefreshAccessToken, "refresh-token");
    }

    [EndpointSummary("Register")]
    [EndpointDescription("Create a new user account.")]
    public static async Task<IResult> Register(RegisterCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        RegisterResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Login")]
    [EndpointDescription("Authenticates a user. Use ?useCookies=true for cookie-based authentication.")]
    public static async Task<IResult> Login(LoginCommand command, ISender sender, CancellationToken cancellationToken)
    {
        LoginResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Logout")]
    [EndpointDescription("Logs out the current user by clearing the authentication cookie.")]
    public static async Task<IResult> Logout(LogoutCommand command, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Ok();
    }

    [EndpointSummary("Refresh token")]
    [EndpointDescription("Returns a new access token using a valid refresh token.")]
    public static async Task<IResult> RefreshAccessToken(RefreshAccessTokenCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        RefreshAccessTokenResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }
}
