using Application.Students.Commands.AddStudent;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Queries.GetStudentInfo;

namespace Web.Endpoints;

public class Students : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(AddStudent)
            .RequireAuthorization()
            .RequireRateLimiting("post");

        groupBuilder.MapGet(GetStudentInfo)
            .RequireAuthorization()
            .RequireRateLimiting("get");

        groupBuilder.MapDelete(DeleteStudent, "{id:int}")
            .RequireAuthorization()
            .RequireRateLimiting("delete");

        groupBuilder.MapPut(UpdateStudent, "")
            .RequireAuthorization()
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Add student")]
    [EndpointDescription("Creates a new student.")]
    public static async Task<IResult> AddStudent(AddStudentCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        int newStudentId = await sender.Send(command, cancellationToken);
        return Results.Ok(newStudentId);
    }

    [EndpointSummary("Get student info")]
    [EndpointDescription("Returns student information.")]
    public static async Task<IResult> GetStudentInfo([AsParameters] GetStudentInfoQuery query, ISender sender,
        CancellationToken cancellationToken)
    {
        StudentInfoVm vm = await sender.Send(query, cancellationToken);
        return Results.Ok(vm);
    }

    [EndpointSummary("Delete student")]
    [EndpointDescription("Deletes a student by id.")]
    public static async Task<IResult> DeleteStudent(int id, ISender sender, CancellationToken cancellationToken)
    {
        DeleteStudentCommand command = new(id);
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Update student")]
    [EndpointDescription("Updates student information.")]
    public static async Task<IResult> UpdateStudent(UpdateStudentCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
