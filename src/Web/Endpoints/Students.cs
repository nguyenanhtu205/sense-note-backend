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
    [EndpointDescription("""
                         Creates a new student.

                         StudentSensitivityProfile fields:
                         - SoundSensitivity: Sensitivity to noise (higher value = more sensitive).
                         - LightSensitivity: Sensitivity to light intensity or sudden light changes.
                         - TemperatureSensitivity: Sensitivity to hot/cold environments.
                         - TouchSensitivity: Sensitivity to physical contact or proximity.
                         - Distractibility: Tendency to be distracted by surroundings (higher value = more distractible).
                         - SensitiveTimeSlots: Time ranges when the student is more sensitive (e.g. '08:00-09:00').
                         - OverallSensitivityLevel: Overall sensitivity level (optional aggregated score).
                         - MedicalNotes: Free-text notes from teacher; can be used to infer other sensitivity fields.
                         - If the sensitivity fields are not provided, the system will automatically infer
                         SoundSensitivity, LightSensitivity, TemperatureSensitivity, TouchSensitivity,
                         and Distractibility based on the content of MedicalNotes.
                         - Response include student id and list of sensitive locations of this student.
                         """)]
    public static async Task<IResult> AddStudent(AddStudentCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        AddStudentResponse result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
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
    [EndpointDescription("""
                         Updates student information.

                         StudentSensitivityProfile:
                         - Clients may update specific sensitivity fields if values are known.
                         - If unsure how to set sensitivity values, it is sufficient to provide MedicalNotes only.
                         - When sensitivity fields are omitted, the system will automatically infer
                           SoundSensitivity, LightSensitivity, TemperatureSensitivity, TouchSensitivity,
                           and Distractibility based on MedicalNotes.
                         - Only changed values will be recorded, and history is preserved.
                         """)]
    public static async Task<IResult> UpdateStudent(UpdateStudentCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        List<string> result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }
}
