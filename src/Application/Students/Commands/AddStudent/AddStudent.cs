namespace Application.Students.Commands.AddStudent;

public record AddStudentCommand(
    int ClassId,
    string FullName,
    DateTime? BirthDay,
    int TeachingContextId,
    string DisplayName,
    int OrdinalIndex) : IRequest<int>;

public class AddStudentCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AddStudentCommand, int>
{
    public async Task<int> Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        Student newStudent = new()
        {
            ClassId = request.ClassId, FullName = request.FullName, Birthday = request.BirthDay
        };

        SeatAssignment newSeatAssignment = new()
        {
            TeachingContextId = request.TeachingContextId,
            Student = newStudent,
            DisplayName = request.DisplayName,
            OrdinalIndex = request.OrdinalIndex
        };

        context.Students.Add(newStudent);

        context.SeatAssignments.Add(newSeatAssignment);

        newStudent.MarkAsAdded(request.TeachingContextId, request.DisplayName);

        await context.SaveChangesAsync(cancellationToken);

        return newStudent.Id;
    }
}
