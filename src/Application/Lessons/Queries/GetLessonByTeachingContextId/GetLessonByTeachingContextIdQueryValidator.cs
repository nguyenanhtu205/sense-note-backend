namespace Application.Lessons.Queries.GetLessonByTeachingContextId;

public class GetLessonByTeachingContextIdQueryValidator : AbstractValidator<GetLessonByTeachingContextIdQuery>
{
    public GetLessonByTeachingContextIdQueryValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");
    }
}
