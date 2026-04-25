using Application.Seats.Commands.UpdateSeatAssignment;
using Application.Seats.Queries.GetSeatAssignmentsByTeachingContextId;

namespace Web.Endpoints;

public class Seats : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetSeatAssignmentsByTeachingContextId, "{teachingContextId:int}")
            .RequireAuthorization()
            .RequireRateLimiting("get");

        groupBuilder.MapPut(UpdateSeatAssignment, "")
            .RequireAuthorization()
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Get seat assignments")]
    [EndpointDescription("Returns seat assignments for a given teaching context.")]
    public static async Task<IResult> GetSeatAssignmentsByTeachingContextId(int teachingContextId, ISender sender,
        CancellationToken cancellationToken)
    {
        SeatAssignmentsVm vm = await sender.Send(
            new GetSeatAssignmentsByTeachingContextIdQuery(teachingContextId),
            cancellationToken);

        return Results.Ok(vm);
    }

    [EndpointSummary("Update seat assignment")]
    [EndpointDescription("Updates seat assignment information.")]
    public static async Task<IResult> UpdateSeatAssignment(UpdateSeatAssignmentCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
