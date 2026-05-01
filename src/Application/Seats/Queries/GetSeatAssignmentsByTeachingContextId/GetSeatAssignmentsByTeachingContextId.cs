namespace Application.Seats.Queries.GetSeatAssignmentsByTeachingContextId;

public record GetSeatAssignmentsByTeachingContextIdQuery(int TeachingContextId) : IRequest<SeatAssignmentsVm>;

public class GetSeatAssignmentsByTeachingContextIdQueryHandler(
    IApplicationDbContext context,
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

        List<SeatAssignment> seatAssignments = await context.SeatAssignments
            .AsNoTracking()
            .Where(sa => sa.TeachingContextId == request.TeachingContextId)
            .OrderBy(sa => sa.OrdinalIndex)
            .ToListAsync(cancellationToken);

        List<int> studentIds = seatAssignments.Select(sa => sa.StudentId).ToList();

        var sensitiveLocationsByStudentId = await context.Students
            .Where(s => studentIds.Contains(s.Id))
            .Select(s => new
            {
                s.Id,
                Locations = s.StudentSensitivityProfiles
                    .SelectMany(p => p.SensitiveLocations)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        Dictionary<int, List<string>> locationDict = sensitiveLocationsByStudentId
            .ToDictionary(x => x.Id, x => x.Locations);

        List<SeatAssignmentDto> seatAssignmentsDto = seatAssignments.Select(sa => new SeatAssignmentDto
        {
            StudentId = sa.StudentId,
            DisplayName = sa.DisplayName,
            OrdinalIndex = sa.OrdinalIndex,
            SensitiveLocations = locationDict[sa.StudentId]
        }).ToList();

        return new SeatAssignmentsVm { SeatAssignments = seatAssignmentsDto };
    }
}
