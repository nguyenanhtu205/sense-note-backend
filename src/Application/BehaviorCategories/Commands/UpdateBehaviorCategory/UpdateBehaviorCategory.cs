using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.BehaviorCategories.Commands.UpdateBehaviorCategory;

public record UpdateBehaviorCategoryCommand(int Id, string Name, int PointValue) : IRequest;

public class UpdateBehaviorCategoryCommandHandler(
    IApplicationDbContext context,
    ICurrentTeacher currentTeacher)
    : IRequestHandler<UpdateBehaviorCategoryCommand>
{
    public async Task Handle(UpdateBehaviorCategoryCommand request, CancellationToken cancellationToken)
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

        bool isDuplicate = await context.BehaviorCategories
            .AnyAsync(x => x.TeacherId == teacherId
                           && x.Name == request.Name
                           && x.Id != request.Id, cancellationToken);

        if (isDuplicate)
        {
            throw new ValidationException([
                new ValidationFailure("Name",
                    $"Behavior category name {request.Name} already exists.")
            ]);
        }

        behaviorCategory.Name = request.Name;
        behaviorCategory.PointValue = request.PointValue;

        await context.SaveChangesAsync(cancellationToken);
    }
}
