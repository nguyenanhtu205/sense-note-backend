namespace Application.Seats.Queries.GetSeatAssignmentsByTeachingContextId;

public class GetSeatAssignmentsByTeachingContextIdQueryValidator
    : AbstractValidator<GetSeatAssignmentsByTeachingContextIdQuery>
{
    public GetSeatAssignmentsByTeachingContextIdQueryValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0)
            .WithMessage("Teaching context id must be greater than 0");
    }
}
