namespace Application.Lessons.Queries.GetLessonByTeachingContextId;

public record GetLessonByTeachingContextIdQuery(int TeachingContextId) : IRequest<LessonVm>;

public class GetLessonByTeachingContextIdQueryHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<GetLessonByTeachingContextIdQuery, LessonVm>
{
    public async Task<LessonVm> Handle(GetLessonByTeachingContextIdQuery request, CancellationToken cancellationToken)
    {
        TeachingContext? teachingContext = await context.TeachingContexts
            .AsNoTracking()
            .Where(tc => tc.Id == request.TeachingContextId)
            .FirstOrDefaultAsync(cancellationToken);

        if (teachingContext == null)
        {
            throw new NotFoundException($"Teaching context with id {request.TeachingContextId} was not found");
        }

        if (teachingContext.TeacherId != int.Parse(currentTeacher.Id!))
        {
            throw new ForbiddenAccessException("You don't have permission to access this resource");
        }

        return new LessonVm
        {
            Lessons = await context.Lessons
                .AsNoTracking()
                .Where(l => l.TeachingContextId == request.TeachingContextId)
                .Select(l => new LessonItemVm(l.Name, l.Id))
                .ToListAsync(cancellationToken)
        };
    }
}
