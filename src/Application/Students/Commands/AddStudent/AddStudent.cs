namespace Application.Students.Commands.AddStudent;

public record AddStudentResponse(int StudentId, List<string> SensitiveLocations);

public record StudentSensitivityProfile(
    int SoundSensitivity,
    int LightSensitivity,
    int TemperatureSensitivity,
    int TouchSensitivity,
    int Distractibility,
    List<string>? SensitiveTimeSlots,
    int OverallSensitivityLevel,
    string MedicalNotes
);

public record AddStudentCommand(
    int ClassId,
    string FullName,
    DateTime? BirthDay,
    int TeachingContextId,
    string DisplayName,
    StudentSensitivityProfile StudentSensitivityProfile) : IRequest<AddStudentResponse>;

public class AddStudentCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AddStudentCommand, AddStudentResponse>
{
    public async Task<AddStudentResponse> Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        TeachingContext? teachingContext = await context.TeachingContexts
            .FirstOrDefaultAsync(tc => tc.Id == request.TeachingContextId, cancellationToken);

        if (teachingContext == null)
        {
            throw new NotFoundException($"Teaching context with id {request.TeachingContextId} was not found.");
        }

        List<string> sensitiveLocations = Student.CalculateSensitiveLocations(
            request.StudentSensitivityProfile.SoundSensitivity,
            request.StudentSensitivityProfile.LightSensitivity,
            request.StudentSensitivityProfile.TemperatureSensitivity,
            request.StudentSensitivityProfile.TouchSensitivity,
            request.StudentSensitivityProfile.Distractibility,
            teachingContext.NumCols, teachingContext.NumRows, teachingContext.SeatsPerTable,
            teachingContext.EnvironmentalAssets);

        Student newStudent = new()
        {
            ClassId = request.ClassId,
            FullName = request.FullName,
            Birthday = request.BirthDay,
            StudentSensitivityProfiles =
            [
                new Domain.Entities.StudentSensitivityProfile
                {
                    SoundSensitivity = request.StudentSensitivityProfile.SoundSensitivity,
                    LightSensitivity = request.StudentSensitivityProfile.LightSensitivity,
                    TemperatureSensitivity = request.StudentSensitivityProfile.TemperatureSensitivity,
                    TouchSensitivity = request.StudentSensitivityProfile.TouchSensitivity,
                    Distractibility = request.StudentSensitivityProfile.Distractibility,
                    SensitiveTimeSlots = request.StudentSensitivityProfile.SensitiveTimeSlots ?? [],
                    SensitiveLocations = sensitiveLocations,
                    OverallSensitivityLevel = request.StudentSensitivityProfile.OverallSensitivityLevel,
                    MedicalNotes = request.StudentSensitivityProfile.MedicalNotes,
                    LastUpdated = DateTimeOffset.UtcNow
                }
            ]
        };

        SeatAssignment newSeatAssignment = new()
        {
            TeachingContextId = request.TeachingContextId,
            Student = newStudent,
            DisplayName = request.DisplayName,
            OrdinalIndex = -1
        };

        context.Students.Add(newStudent);

        context.SeatAssignments.Add(newSeatAssignment);

        newStudent.MarkAsAdded(request.TeachingContextId, request.DisplayName);

        await context.SaveChangesAsync(cancellationToken);

        return new AddStudentResponse(newStudent.Id, sensitiveLocations);
    }
}
