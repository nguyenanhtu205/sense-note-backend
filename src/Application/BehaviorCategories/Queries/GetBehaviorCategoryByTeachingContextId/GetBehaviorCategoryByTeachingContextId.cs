namespace Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeachingContextId;

public record GetBehaviorCategoriesByTeachingContextIdQuery(int TeachingContextId)
    : IRequest<BehaviorCategoriesVm>;

public class GetBehaviorCategoryByTeachingContextIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetBehaviorCategoriesByTeachingContextIdQuery, BehaviorCategoriesVm>
{
    public async Task<BehaviorCategoriesVm> Handle(GetBehaviorCategoriesByTeachingContextIdQuery request,
        CancellationToken cancellationToken)
    {
        return new BehaviorCategoriesVm
        {
            BehaviorCategories = await context.ContextBehaviorMaps
                .AsNoTracking()
                .Where(x => x.TeachingContextId == request.TeachingContextId)
                .Include(x => x.BehaviorCategory)
                .Select(x => x.BehaviorCategory)
                .ProjectTo<BehaviorCategoryDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}
