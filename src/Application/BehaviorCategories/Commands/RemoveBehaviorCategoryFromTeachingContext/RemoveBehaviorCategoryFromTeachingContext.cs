namespace Application.BehaviorCategories.Commands.RemoveBehaviorCategoryFromTeachingContext;

public record RemoveBehaviorCategoryFromTeachingContextCommand(int TeachingContextId, List<int> BehaviorCategoryId)
    : IRequest;

public class RemoveBehaviorCategoryFromTeachingContextCommandHandler(IApplicationDbContext context)
    : IRequestHandler<RemoveBehaviorCategoryFromTeachingContextCommand>
{
    public async Task Handle(RemoveBehaviorCategoryFromTeachingContextCommand request,
        CancellationToken cancellationToken)
    {
        List<ContextBehaviorMap> mapsToRemove = await context.ContextBehaviorMaps
            .Where(x => x.TeachingContextId == request.TeachingContextId
                        && request.BehaviorCategoryId.Contains(x.BehaviorCategoryId))
            .ToListAsync(cancellationToken);

        context.ContextBehaviorMaps.RemoveRange(mapsToRemove);

        await context.SaveChangesAsync(cancellationToken);
    }
}
