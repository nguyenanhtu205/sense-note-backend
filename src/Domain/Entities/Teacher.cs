namespace Domain.Entities;

public class Teacher : BaseAuditableEntity
{
    public required string FullName { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public ICollection<Class> Classes { get; private set; } = new List<Class>();

    public ICollection<TeachingContext> TeachingContexts { get; private set; } = new List<TeachingContext>();

    public ICollection<BehaviorCategory> BehaviorCategories { get; private set; } = new List<BehaviorCategory>();

    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
}
