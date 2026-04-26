namespace Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeacherId;

public record GetBehaviorCategoryByTeacherIdQuery : IRequest<BehaviorCategoriesVm>;

public class GetBehaviorCategoryByTeacherIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentTeacher currentTeacher)
    : IRequestHandler<GetBehaviorCategoryByTeacherIdQuery, BehaviorCategoriesVm>
{
    public async Task<BehaviorCategoriesVm> Handle(GetBehaviorCategoryByTeacherIdQuery request,
        CancellationToken cancellationToken)
    {
        return new BehaviorCategoriesVm
        {
            BehaviorCategories = await context.BehaviorCategories
                .AsNoTracking()
                .Where(bc => bc.TeacherId == int.Parse(currentTeacher.Id!) && bc.DeletedAt == null)
                .OrderBy(bc => bc.Id)
                .ProjectTo<BehaviorCategoryDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}
