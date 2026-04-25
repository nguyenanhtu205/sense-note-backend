using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Lessons.Commands.StartLesson;

public record StartLessonCommand(int TeachingContextId, string LessonName) : IRequest<int>;

public class StartLessonCommandHandler(IApplicationDbContext context)
    : IRequestHandler<StartLessonCommand, int>
{
    public async Task<int> Handle(StartLessonCommand request, CancellationToken cancellationToken)
    {
        TeachingContext? teachingContext = await context.TeachingContexts
            .Include(tc => tc.Lessons)
            .FirstOrDefaultAsync(tc => tc.Id == request.TeachingContextId, cancellationToken);

        if (teachingContext == null)
        {
            throw new NotFoundException($"Teaching context with id {request.TeachingContextId} was not found");
        }

        bool hasOngoingLesson = teachingContext.Lessons
            .Any(lesson => lesson.LessonStatus == LessonStatus.Ongoing);

        if (hasOngoingLesson)
        {
            throw new ValidationException([
                new ValidationFailure("Status",
                    "There is already an ongoing lesson in this teaching context")
            ]);
        }

        Lesson lesson = new()
        {
            TeachingContextId = request.TeachingContextId,
            Name = request.LessonName,
            StartAt = DateTimeOffset.UtcNow,
            LessonStatus = LessonStatus.Ongoing
        };

        context.Lessons.Add(lesson);

        await context.SaveChangesAsync(cancellationToken);

        return lesson.Id;
    }
}
