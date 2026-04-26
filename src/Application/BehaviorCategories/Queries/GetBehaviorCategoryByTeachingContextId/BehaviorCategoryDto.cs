namespace Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeachingContextId;

public class BehaviorCategoryDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public int PointValue { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<BehaviorCategory, BehaviorCategoryDto>();
        }
    }
}
