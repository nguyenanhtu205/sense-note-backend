using Application.Lessons.Commands.EndLesson;
using Application.Lessons.Commands.StartLesson;

namespace Web.Endpoints;

public class Lessons : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(StartLesson)
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPut(EndLesson, "{lessonId:int}")
            .RequireAuthorization()
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Start lesson")]
    [EndpointDescription("Starts a lesson and enables behavior tracking functionality.")]
    public static async Task<IResult> StartLesson(StartLessonCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        int lessonId = await sender.Send(command, cancellationToken);

        return Results.Ok(lessonId);
    }

    [EndpointSummary("End lesson")]
    [EndpointDescription("Ends an ongoing lesson and stops behavior tracking")]
    public static async Task<IResult> EndLesson(int lessonId, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(new EndLessonCommand(lessonId), cancellationToken);

        return Results.NoContent();
    }
}
