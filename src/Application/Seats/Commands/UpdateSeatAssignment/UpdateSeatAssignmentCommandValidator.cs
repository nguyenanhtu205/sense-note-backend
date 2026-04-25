namespace Application.Seats.Commands.UpdateSeatAssignment;

public class UpdateSeatAssignmentCommandValidator : AbstractValidator<UpdateSeatAssignmentCommand>
{
    public UpdateSeatAssignmentCommandValidator()
    {
        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0)
            .WithMessage("Teaching context id must be greater than 0");

        RuleFor(x => x.Seats)
            .NotNull().WithMessage("Seats must not be null")
            .NotEmpty().WithMessage("Seats must not be empty");

        RuleForEach(x => x.Seats).ChildRules(seat =>
        {
            seat.RuleFor(s => s.StudentId)
                .GreaterThan(0).WithMessage("Student id must be greater than 0");
            seat.RuleFor(s => s.OrdinalIndex)
                .Must(x => x is -1 or > 0)
                .WithMessage("Ordinal index must be -1 or greater than 0");
        });

        RuleFor(x => x.Seats)
            .Must(seats => seats
                .GroupBy(s => s.StudentId)
                .All(g => g.Count() == 1))
            .WithMessage("Duplicate student id in Seats");

        RuleFor(x => x.Seats)
            .Must(seats => seats
                .Where(s => s.OrdinalIndex > 0)
                .GroupBy(s => s.OrdinalIndex)
                .All(g => g.Count() == 1))
            .WithMessage("Duplicate ordinal index in Seats");
    }
}
