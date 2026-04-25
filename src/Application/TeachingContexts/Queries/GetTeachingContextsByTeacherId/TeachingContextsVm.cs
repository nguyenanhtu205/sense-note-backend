namespace Application.TeachingContexts.Queries.GetTeachingContextsByTeacherId;

public class TeachingContextsVm
{
    public IReadOnlyCollection<TeachingContextDto> TeachingContexts { get; init; } = [];
}
