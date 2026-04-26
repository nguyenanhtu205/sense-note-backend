namespace Application.BehaviorLogs.Queries.GetStudentBehaviorLogHistory;

public class GetStudentBehaviorLogHistoryQueryValidator : AbstractValidator<GetStudentBehaviorLogHistoryQuery>
{
    public GetStudentBehaviorLogHistoryQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student id must be greater than 0.");

        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("Lesson id must be greater than 0.");
    }
}
