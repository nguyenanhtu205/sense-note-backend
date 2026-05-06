using Application.BehaviorLogs.Commands.LogBehavior;
using Application.BehaviorLogs.Queries.GetClassBehaviorLogHistory;
using Application.BehaviorLogs.Queries.GetStudentBehaviorLogHistory;

namespace Web.Endpoints;

public record LogBehaviorResponse(int LogId);

public class BehaviorLogs : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(LogBehavior)
            .Produces<LogBehaviorResponse>()
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapGet(GetStudentBehaviorLogHistory, "student")
            .Produces<StudentBehaviorLogHistoryVm>()
            .RequireAuthorization()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetClassBehaviorLogHistory, "class")
            .Produces<ClassBehaviorLogHistoryVm>()
            .RequireAuthorization()
            .RequireRateLimiting("get");
    }

    [EndpointSummary("Log behavior")]
    [EndpointDescription("Logs a behavior for a student.")]
    public static async Task<IResult> LogBehavior(LogBehaviorCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        int logId = await sender.Send(command, cancellationToken);
        return Results.Ok(new LogBehaviorResponse(logId));
    }

    [EndpointSummary("Get student behavior log history")]
    [EndpointDescription("Returns behavior log history for a student.")]
    public static async Task<IResult> GetStudentBehaviorLogHistory(
        [AsParameters] GetStudentBehaviorLogHistoryQuery query, ISender sender, CancellationToken cancellationToken)
    {
        StudentBehaviorLogHistoryVm vm = await sender.Send(query, cancellationToken);
        return Results.Ok(vm);
    }

    [EndpointSummary("Get class behavior log history")]
    [EndpointDescription("Returns behavior log history for a class.")]
    public static async Task<IResult> GetClassBehaviorLogHistory([AsParameters] GetClassBehaviorLogHistoryQuery query,
        ISender sender, CancellationToken cancellationToken)
    {
        ClassBehaviorLogHistoryVm vm = await sender.Send(query, cancellationToken);
        return Results.Ok(vm);
    }
}
