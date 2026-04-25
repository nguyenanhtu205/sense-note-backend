namespace Application.Students.Commands.UpdateStudent;

public record UpdateStudentCommand(
    int StudentId,
    int TeachingContextId,
    string FullName,
    DateTime? BirthDay,
    string DisplayName) : IRequest;

public class UpdateStudentCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateStudentCommand>
{
    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        Student? studentToUpdate = await context.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (studentToUpdate == null)
        {
            throw new NotFoundException($"Student with id {request.StudentId} was not found.");
        }

        SeatAssignment? seatAssignment = await context.SeatAssignments
            .FirstOrDefaultAsync(sa =>
                    sa.TeachingContextId == request.TeachingContextId &&
                    sa.StudentId == request.StudentId,
                cancellationToken);

        if (seatAssignment == null)
        {
            throw new NotFoundException(
                $"Seat of student with id {request.StudentId} in teaching context with id {request.TeachingContextId} was not found.");
        }

        bool isChanged =
            studentToUpdate.FullName != request.FullName ||
            studentToUpdate.Birthday != request.BirthDay ||
            seatAssignment.DisplayName != request.DisplayName;

        if (!isChanged)
        {
            return;
        }

        studentToUpdate.UpdateInfo(request.FullName, request.BirthDay);
        seatAssignment.UpdateDisplayName(request.DisplayName);

        await context.SaveChangesAsync(cancellationToken);
    }
}
