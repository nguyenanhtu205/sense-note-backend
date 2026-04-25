namespace Application.TeachingContexts.Queries.GetTeachingContextsByTeacherId;

public record GetTeachingContextsByTeacherIdQuery : IRequest<TeachingContextsVm>;

public class GetTeachingContextsByTeacherIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentTeacher currentTeacher)
    : IRequestHandler<GetTeachingContextsByTeacherIdQuery, TeachingContextsVm>
{
    public async Task<TeachingContextsVm> Handle(GetTeachingContextsByTeacherIdQuery request,
        CancellationToken cancellationToken)
    {
        return new TeachingContextsVm
        {
            TeachingContexts = await context.TeachingContexts
                .AsNoTracking()
                .Where(tc => tc.TeacherId == int.Parse(currentTeacher.Id!) && tc.DeletedAt == null)
                .OrderBy(tc => tc.Id)
                .ProjectTo<TeachingContextDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}
