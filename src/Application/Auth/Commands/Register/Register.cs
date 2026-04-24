namespace Application.Auth.Commands.Register;

public record RegisterResponse(string AccessToken, string RefreshToken);

public record RegisterCommand(string FullName, string Email, string Password)
    : IRequest<RegisterResponse>;

public class RegisterCommandHandler(
    IApplicationDbContext context,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenHasher refreshTokenHasher)
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        bool emailExists = await context.Teachers
            .AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            throw new ConflictException("Email already exists");
        }

        Teacher teacher = new()
        {
            FullName = request.FullName, Email = request.Email, PasswordHash = passwordHasher.Hash(request.Password)
        };

        string refreshTokenValue = refreshTokenGenerator.Generate();

        RefreshToken refreshToken = new()
        {
            Teacher = teacher, Token = refreshTokenHasher.Hash(refreshTokenValue), IsRevoked = false
        };

        context.Teachers.Add(teacher);

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(jwtProvider.Generate(teacher), refreshTokenValue);
    }
}
