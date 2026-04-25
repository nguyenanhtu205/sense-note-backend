namespace Application.TeachingContexts.Queries.GetTeachingContextsByTeacherId;

public class TeachingContextDto
{
    public int Id { get; init; }

    public required string ContextName { get; init; }

    public int NumCols { get; init; }

    public int NumRows { get; init; }

    public int SeatsPerTable { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TeachingContext, TeachingContextDto>();
        }
    }
}
