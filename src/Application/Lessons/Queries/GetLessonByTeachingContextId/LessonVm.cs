namespace Application.Lessons.Queries.GetLessonByTeachingContextId;

public record LessonItemVm(string Name, int LessonId);

public class LessonVm
{
    public IReadOnlyCollection<LessonItemVm> Lessons { get; init; } = [];
}
