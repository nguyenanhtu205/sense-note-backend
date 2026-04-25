namespace Application.Lessons.Commands.EndLesson;

public record EndLessonCommand(int LessonId) : IRequest;

public class EndLessonCommandHandler(IApplicationDbContext context) : IRequestHandler<EndLessonCommand>
{
    public async Task Handle(EndLessonCommand request, CancellationToken cancellationToken)
    {
        Lesson? lesson = await context.Lessons.FirstOrDefaultAsync(x => x.Id == request.LessonId, cancellationToken);

        if (lesson == null)
        {
            throw new NotFoundException($"Lesson with id {request.LessonId} was not found");
        }

        lesson.EndAt = DateTimeOffset.UtcNow;
        lesson.LessonStatus = LessonStatus.Inactive;

        await context.SaveChangesAsync(cancellationToken);
    }
}
