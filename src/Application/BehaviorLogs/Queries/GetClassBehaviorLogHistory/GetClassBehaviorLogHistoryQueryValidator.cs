namespace Application.BehaviorLogs.Queries.GetClassBehaviorLogHistory;

public class GetClassBehaviorLogHistoryQueryValidator : AbstractValidator<GetClassBehaviorLogHistoryQuery>
{
    public GetClassBehaviorLogHistoryQueryValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0.");

        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("Lesson id must be greater than 0.");
    }
}
