namespace Application.BehaviorCategories.Commands.ApplyBehaviorCategoryToTeachingContext;

public record ApplyBehaviorCategoryToTeachingContextCommand(int TeachingContextId, List<int> BehaviorCategoryId)
    : IRequest;

public class ApplyBehaviorCategoryToTeachingContextCommandHandler(IApplicationDbContext context)
    : IRequestHandler<ApplyBehaviorCategoryToTeachingContextCommand>
{
    public async Task Handle(ApplyBehaviorCategoryToTeachingContextCommand request, CancellationToken cancellationToken)
    {
        List<ContextBehaviorMap> maps = request.BehaviorCategoryId.Select(id => new ContextBehaviorMap
        {
            TeachingContextId = request.TeachingContextId, BehaviorCategoryId = id
        }).ToList();

        context.ContextBehaviorMaps.AddRange(maps);

        await context.SaveChangesAsync(cancellationToken);
    }
}
