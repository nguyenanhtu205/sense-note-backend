namespace Application.Auth.Commands.Login;

public record LoginResponse(string AccessToken, string RefreshToken);

public record LoginCommand(string Email, string Password)
    : IRequest<LoginResponse>;

public class Login(
    IApplicationDbContext context,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenHasher refreshTokenHasher) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Teacher? teacher = await context.Teachers
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (teacher is null)
        {
            throw new NotFoundException($"Email {request.Email} does not exist");
        }

        if (!passwordHasher.Verify(request.Password, teacher.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid password");
        }

        string refreshTokenValue = refreshTokenGenerator.Generate();
        RefreshToken refreshToken = new()
        {
            Teacher = teacher, Token = refreshTokenHasher.Hash(refreshTokenValue), IsRevoked = false
        };

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(jwtProvider.Generate(teacher), refreshTokenValue);
    }
}
