namespace Application.BehaviorCategories.Commands.DeleteBehaviorCategory;

public record DeleteBehaviorCategoryCommand(int Id) : IRequest;

public class DeleteBehaviorCategoryCommandHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<DeleteBehaviorCategoryCommand>
{
    public async Task Handle(DeleteBehaviorCategoryCommand request, CancellationToken cancellationToken)
    {
        string? currentTeacherId = currentTeacher.Id;

        if (currentTeacherId == null)
        {
            throw new UnauthorizedAccessException();
        }

        int teacherId = int.Parse(currentTeacherId);

        BehaviorCategory? behaviorCategory = await context.BehaviorCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (behaviorCategory == null)
        {
            throw new NotFoundException($"Behavior category with id {request.Id} was not found");
        }

        if (behaviorCategory.TeacherId != teacherId)
        {
            throw new ForbiddenAccessException("You do not have permission to access this resource");
        }

        behaviorCategory.DeletedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
