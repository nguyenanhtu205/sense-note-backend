namespace Application.Auth.Commands.Logout;

public record LogoutCommand : IRequest;

public class LogoutCommandHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        string? currentTeacherId = currentTeacher.Id;
        if (currentTeacherId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }

        int currentTeacherIdValue = int.Parse(currentTeacherId);

        await context.RefreshTokens
            .Where(rt => rt.TeacherId == currentTeacherIdValue && !rt.IsRevoked)
            .ExecuteUpdateAsync(
                s => s.SetProperty(x => x.IsRevoked, true),
                cancellationToken);
    }
}
