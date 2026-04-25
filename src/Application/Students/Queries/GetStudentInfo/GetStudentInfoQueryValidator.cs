namespace Application.Students.Queries.GetStudentInfo;

public class GetStudentInfoQueryValidator : AbstractValidator<GetStudentInfoQuery>
{
    public GetStudentInfoQueryValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0)
            .WithMessage("Teaching context id must be greater than 0");

        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student id must be greater than 0");
    }
}
