namespace Application.Seats.Commands.UpdateSeatAssignment;

public record SeatUpdateItem(int StudentId, int OrdinalIndex);

public record UpdateSeatAssignmentCommand : IRequest
{
    public int TeachingContextId { get; init; }
    public List<SeatUpdateItem> Seats { get; init; } = [];
}

public class UpdateSeatAssignmentCommandHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<UpdateSeatAssignmentCommand>
{
    public async Task Handle(UpdateSeatAssignmentCommand request, CancellationToken cancellationToken)
    {
        int? teacherId = await context.TeachingContexts
            .Where(tc => tc.Id == request.TeachingContextId)
            .Select(tc => (int?)tc.TeacherId)
            .FirstOrDefaultAsync(cancellationToken);

        if (teacherId == null)
        {
            throw new NotFoundException("Teaching context not found");
        }

        int currentTeacherId = int.Parse(currentTeacher.Id!);

        if (teacherId != currentTeacherId)
        {
            throw new ForbiddenAccessException();
        }

        List<int> studentIds = request.Seats.Select(s => s.StudentId).ToList();

        List<SeatAssignment> seatAssignments = await context.SeatAssignments
            .Where(sa => sa.TeachingContextId == request.TeachingContextId && studentIds.Contains(sa.StudentId))
            .ToListAsync(cancellationToken);

        Dictionary<int, int> seatDict = request.Seats.ToDictionary(
            x => x.StudentId,
            x => x.OrdinalIndex
        );

        foreach (SeatAssignment seat in seatAssignments)
        {
            seat.OrdinalIndex = -1;
        }

        await context.SaveChangesAsync(cancellationToken);

        foreach (SeatAssignment seat in seatAssignments)
        {
            seat.OrdinalIndex = seatDict[seat.StudentId];
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
