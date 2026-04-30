namespace Application.Seats.Queries.GetSeatAssignmentsByTeachingContextId;

public class SeatAssignmentDto
{
    public int StudentId { get; init; }

    public required string DisplayName { get; init; }

    public int OrdinalIndex { get; init; }

    public List<string> SensitiveLocations { get; init; } = [];
}
