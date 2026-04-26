namespace Application.BehaviorCategories.Commands.CreateBehaviorCategory;

public record CreateBehaviorCategoryCommand(string Name, int PointValue) : IRequest<int>;

public class CreateBehaviorCategoryCommandHandler(IApplicationDbContext context, ICurrentTeacher currentTeacher)
    : IRequestHandler<CreateBehaviorCategoryCommand, int>
{
    public async Task<int> Handle(CreateBehaviorCategoryCommand request, CancellationToken cancellationToken)
    {
        string? currentTeacherId = currentTeacher.Id;

        if (currentTeacherId == null)
        {
            throw new UnauthorizedAccessException();
        }

        int teacherId = int.Parse(currentTeacherId);

        BehaviorCategory behaviorCategory = new()
        {
            TeacherId = teacherId, Name = request.Name, PointValue = request.PointValue
        };

        context.BehaviorCategories.Add(behaviorCategory);

        await context.SaveChangesAsync(cancellationToken);

        return behaviorCategory.Id;
    }
}
