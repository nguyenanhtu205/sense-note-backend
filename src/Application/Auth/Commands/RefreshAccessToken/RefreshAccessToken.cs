namespace Application.Auth.Commands.RefreshAccessToken;

public record RefreshAccessTokenResponse(string AccessToken, string RefreshToken);

public record RefreshAccessTokenCommand(string RefreshToken) : IRequest<RefreshAccessTokenResponse>;

public class RefreshAccessTokenCommandHandler(
    IApplicationDbContext context,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenHasher refreshTokenHasher,
    IJwtProvider jwtProvider)
    : IRequestHandler<RefreshAccessTokenCommand, RefreshAccessTokenResponse>
{
    public async Task<RefreshAccessTokenResponse> Handle(RefreshAccessTokenCommand request,
        CancellationToken cancellationToken)
    {
        string token = refreshTokenHasher.Hash(request.RefreshToken);

        RefreshToken? refreshToken = await context.RefreshTokens
            .Include(x => x.Teacher)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (refreshToken == null)
        {
            throw new NotFoundException($"Refresh token {request.RefreshToken} does not exist");
        }

        if (refreshToken.Teacher == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        if (refreshToken.IsRevoked)
        {
            throw new UnauthorizedAccessException($"Refresh token {request.RefreshToken} is revoked");
        }

        if (refreshToken.ExpiredAt < DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedAccessException($"Refresh token {request.RefreshToken} is expired");
        }

        string refreshTokenValue = refreshTokenGenerator.Generate();
        RefreshToken newRefreshToken = new()
        {
            Teacher = refreshToken.Teacher, Token = refreshTokenHasher.Hash(refreshTokenValue), IsRevoked = false
        };

        refreshToken.IsRevoked = true;
        refreshToken.ReplacedByToken = newRefreshToken;

        context.RefreshTokens.Add(newRefreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return new RefreshAccessTokenResponse(jwtProvider.Generate(refreshToken.Teacher), refreshTokenValue);
    }
}
