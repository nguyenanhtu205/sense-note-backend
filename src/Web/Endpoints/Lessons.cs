using Application.Lessons.Commands.EndLesson;
using Application.Lessons.Commands.StartLesson;
using Application.Lessons.Queries.GetLessonByTeachingContextId;

namespace Web.Endpoints;

public record StartLessonResponse(int LessonId);

public class Lessons : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetLessonByTeachingContextId, "{teachingContextId:int}")
            .Produces<LessonVm>()
            .RequireAuthorization()
            .RequireRateLimiting("get");

        groupBuilder.MapPost(StartLesson)
            .Produces<StartLessonResponse>()
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPut(EndLesson, "{lessonId:int}")
            .RequireAuthorization()
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Get lesson by teaching context id")]
    [EndpointDescription("Get all lessons of teaching context")]
    public static async Task<IResult> GetLessonByTeachingContextId(int teachingContextId, ISender sender,
        CancellationToken cancellationToken)
    {
        LessonVm result =
            await sender.Send(new GetLessonByTeachingContextIdQuery(teachingContextId), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Start lesson")]
    [EndpointDescription("Starts a lesson and enables behavior tracking functionality.")]
    public static async Task<IResult> StartLesson(StartLessonCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        int lessonId = await sender.Send(command, cancellationToken);
        return Results.Ok(new StartLessonResponse(lessonId));
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
