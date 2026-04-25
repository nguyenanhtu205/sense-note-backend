namespace Application.Seats.Queries.GetSeatAssignmentsByTeachingContextId;

public class SeatAssignmentsVm
{
    public IReadOnlyCollection<SeatAssignmentDto> SeatAssignments { get; init; } = [];
}
