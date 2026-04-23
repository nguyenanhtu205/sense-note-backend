namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<BehaviorCategory> BehaviorCategories { get; }

    DbSet<BehaviorLog> BehaviorLogs { get; }

    DbSet<Class> Classes { get; }

    DbSet<ContextBehaviorMap> ContextBehaviorMaps { get; }

    DbSet<Lesson> Lessons { get; }

    DbSet<LessonSummary> LessonSummaries { get; }

    DbSet<SeatAssignment> SeatAssignments { get; }

    DbSet<ShareCode> ShareCodes { get; }

    DbSet<Student> Students { get; }

    DbSet<Teacher> Teachers { get; }

    DbSet<TeachingContext> TeachingContexts { get; }

    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
