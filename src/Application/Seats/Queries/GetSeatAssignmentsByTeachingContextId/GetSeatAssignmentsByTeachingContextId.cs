namespace Application.Seats.Queries.GetSeatAssignmentsByTeachingContextId;

public record GetSeatAssignmentsByTeachingContextIdQuery(int TeachingContextId) : IRequest<SeatAssignmentsVm>;

public class GetSeatAssignmentsByTeachingContextIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentTeacher currentTeacher)
    : IRequestHandler<GetSeatAssignmentsByTeachingContextIdQuery, SeatAssignmentsVm>
{
    public async Task<SeatAssignmentsVm> Handle(GetSeatAssignmentsByTeachingContextIdQuery request,
        CancellationToken cancellationToken)
    {
        int teacherId = await context.TeachingContexts
            .Where(tc => tc.Id == request.TeachingContextId)
            .Select(tc => tc.TeacherId)
            .FirstOrDefaultAsync(cancellationToken);

        if (teacherId != int.Parse(currentTeacher.Id!))
        {
            throw new ForbiddenAccessException();
        }

        return new SeatAssignmentsVm
        {
            SeatAssignments = await context.SeatAssignments
                .AsNoTracking()
                .Where(sa => sa.TeachingContextId == request.TeachingContextId)
                .OrderBy(sa => sa.OrdinalIndex)
                .ProjectTo<SeatAssignmentDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}
