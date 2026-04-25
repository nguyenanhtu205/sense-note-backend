namespace Application.TeachingContexts.Commands.CreateTeachingContext;

public record CreateTeachingContextCommand(
    string ClassName,
    string TeachingContextName,
    int NumCols,
    int NumRows,
    int SeatsPerTable) : IRequest<int>;

public class CreateTeachingContextCommandHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<CreateTeachingContextCommand, int>
{
    public async Task<int> Handle(CreateTeachingContextCommand request, CancellationToken cancellationToken)
    {
        string? teacherId = currentTeacher.Id;

        if (teacherId == null)
        {
            throw new UnauthorizedAccessException();
        }

        int teacherIdValue = int.Parse(teacherId);

        Class newClass = new() { Name = request.ClassName, CreatedBy = teacherIdValue };

        TeachingContext newTeachingContext = new()
        {
            TeacherId = teacherIdValue,
            Class = newClass,
            ContextName = request.TeachingContextName,
            NumCols = request.NumCols,
            NumRows = request.NumRows,
            SeatsPerTable = request.SeatsPerTable
        };

        context.TeachingContexts.Add(newTeachingContext);

        await context.SaveChangesAsync(cancellationToken);

        return newTeachingContext.Id;
    }
}
