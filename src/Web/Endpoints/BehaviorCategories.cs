using Application.BehaviorCategories.Commands.ApplyBehaviorCategoryToTeachingContext;
using Application.BehaviorCategories.Commands.CreateBehaviorCategory;
using Application.BehaviorCategories.Commands.DeleteBehaviorCategory;
using Application.BehaviorCategories.Commands.RemoveBehaviorCategoryFromTeachingContext;
using Application.BehaviorCategories.Commands.UpdateBehaviorCategory;
using Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeacherId;
using Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeachingContextId;
using Microsoft.AspNetCore.Mvc;
using TeacherCategoriesVm = Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeacherId.BehaviorCategoriesVm;
using ContextCategoriesVm =
    Application.BehaviorCategories.Queries.GetBehaviorCategoryByTeachingContextId.BehaviorCategoriesVm;

namespace Web.Endpoints;

public record CreateBehaviorCategoryResponse(int NewBehaviorCategoryId);

public class BehaviorCategories : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateBehaviorCategory)
            .Produces<CreateBehaviorCategoryResponse>()
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapPut(UpdateBehaviorCategory, "")
            .RequireAuthorization()
            .RequireRateLimiting("put");

        groupBuilder.MapDelete(DeleteBehaviorCategory, "{id:int}")
            .RequireAuthorization()
            .RequireRateLimiting("delete");

        groupBuilder.MapPost(ApplyBehaviorCategoryToTeachingContext, "apply")
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapDelete(RemoveBehaviorCategoryFromTeachingContext, "remove")
            .RequireAuthorization()
            .RequireRateLimiting("delete");

        groupBuilder.MapGet(GetBehaviorCategoriesByTeacherId)
            .Produces<TeacherCategoriesVm>()
            .RequireAuthorization()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetBehaviorCategoriesByTeachingContextId, "by-teaching-context/{teachingContextId:int}")
            .Produces<ContextCategoriesVm>()
            .RequireAuthorization()
            .RequireRateLimiting("get");
    }

    [EndpointSummary("Create behavior category")]
    [EndpointDescription("Creates a new behavior category.")]
    public static async Task<IResult> CreateBehaviorCategory(CreateBehaviorCategoryCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        int id = await sender.Send(command, cancellationToken);
        return Results.Ok(new CreateBehaviorCategoryResponse(id));
    }

    [EndpointSummary("Update behavior category")]
    [EndpointDescription("Updates an existing behavior category.")]
    public static async Task<IResult> UpdateBehaviorCategory(UpdateBehaviorCategoryCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Delete behavior category")]
    [EndpointDescription("Deletes a behavior category by id.")]
    public static async Task<IResult> DeleteBehaviorCategory(int id, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteBehaviorCategoryCommand(id), cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Apply behavior categories to teaching context")]
    [EndpointDescription("Assigns one or more behavior categories to a teaching context.")]
    public static async Task<IResult> ApplyBehaviorCategoryToTeachingContext(
        [FromBody] ApplyBehaviorCategoryToTeachingContextCommand command,
        ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Remove behavior categories from teaching context")]
    [EndpointDescription("Removes one or more behavior categories from a teaching context.")]
    public static async Task<IResult> RemoveBehaviorCategoryFromTeachingContext(
        [FromBody] RemoveBehaviorCategoryFromTeachingContextCommand command,
        ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Get behavior categories by teacher")]
    [EndpointDescription("Returns all behavior categories for the current teacher.")]
    public static async Task<IResult> GetBehaviorCategoriesByTeacherId(ISender sender,
        CancellationToken cancellationToken)
    {
        TeacherCategoriesVm result = await sender.Send(new GetBehaviorCategoryByTeacherIdQuery(), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get behavior categories by teaching context")]
    [EndpointDescription("Returns all behavior categories for a specific teaching context.")]
    public static async Task<IResult> GetBehaviorCategoriesByTeachingContextId(int teachingContextId, ISender sender,
        CancellationToken cancellationToken)
    {
        ContextCategoriesVm result =
            await sender.Send(new GetBehaviorCategoriesByTeachingContextIdQuery(teachingContextId), cancellationToken);
        return Results.Ok(result);
    }
}
