using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.BehaviorLogs.Commands.LogBehavior;

public record LogBehaviorCommand(int LessonId, int StudentId, int BehaviorCategoryId) : IRequest<int>;

public class LogBehaviorCommandCommandHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<LogBehaviorCommand, int>
{
    public async Task<int> Handle(LogBehaviorCommand request, CancellationToken cancellationToken)
    {
        Lesson? lesson = await context.Lessons
            .FirstOrDefaultAsync(x => x.Id == request.LessonId, cancellationToken);

        if (lesson == null)
        {
            throw new NotFoundException($"Lesson with id {request.LessonId} not found");
        }

        if (lesson.LessonStatus == LessonStatus.Inactive)
        {
            throw new ValidationException([
                new ValidationFailure("Lesson status",
                    "Lesson finished or hadn't started yet, can't log behavior")
            ]);
        }

        bool studentExists = await context.Students.AnyAsync(x => x.Id == request.StudentId, cancellationToken);

        if (!studentExists)
        {
            throw new NotFoundException($"Student with id {request.StudentId} not found");
        }

        BehaviorCategory? behaviorCategory = await context.BehaviorCategories
            .FirstOrDefaultAsync(
                x => x.Id == request.BehaviorCategoryId && x.DeletedAt == null, cancellationToken);

        if (behaviorCategory == null)
        {
            throw new NotFoundException($"Behavior category with id {request.BehaviorCategoryId} not found");
        }

        if (behaviorCategory.TeacherId != int.Parse(currentTeacher.Id!))
        {
            throw new ForbiddenAccessException("You don't have permission to access this resource");
        }

        BehaviorLog log = new()
        {
            LessonId = request.LessonId,
            StudentId = request.StudentId,
            BehaviorCategoryId = request.BehaviorCategoryId,
            OccurredAt = DateTimeOffset.UtcNow
        };

        context.BehaviorLogs.Add(log);

        LessonSummary? summary = await context.LessonSummaries.FirstOrDefaultAsync(
            x => x.LessonId == request.LessonId && x.StudentId == request.StudentId, cancellationToken);

        if (summary == null)
        {
            summary = new LessonSummary
            {
                LessonId = request.LessonId, StudentId = request.StudentId, FinalScore = behaviorCategory.PointValue
            };

            context.LessonSummaries.Add(summary);
        }
        else
        {
            summary.FinalScore = (summary.FinalScore ?? 0) + behaviorCategory.PointValue;
        }

        await context.SaveChangesAsync(cancellationToken);

        return log.Id;
    }
}
