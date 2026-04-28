namespace Application.Students.Commands.AddStudent;

public record StudentSensitivityProfile(
    int? SoundSensitivity,
    int? LightSensitivity,
    int? TemperatureSensitivity,
    int? TouchSensitivity,
    int? Distractibility,
    List<string>? SensitiveTimeSlots,
    int? OverallSensitivityLevel,
    string? MedicalNotes
);

public record AddStudentCommand(
    int ClassId,
    string FullName,
    DateTime? BirthDay,
    int TeachingContextId,
    string DisplayName,
    int OrdinalIndex,
    StudentSensitivityProfile StudentSensitivityProfile) : IRequest<int>;

public class AddStudentCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AddStudentCommand, int>
{
    public async Task<int> Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        TeachingContext? teachingContext = await context.TeachingContexts
            .FirstOrDefaultAsync(tc => tc.Id == request.TeachingContextId, cancellationToken);

        if (teachingContext == null)
        {
            throw new NotFoundException($"Teaching context with id {request.TeachingContextId} was not found.");
        }

        Student newStudent = new()
        {
            ClassId = request.ClassId,
            FullName = request.FullName,
            Birthday = request.BirthDay,
            StudentSensitivityProfiles =
            [
                new Domain.Entities.StudentSensitivityProfile
                {
                    SoundSensitivity = request.StudentSensitivityProfile.SoundSensitivity ?? 0,
                    LightSensitivity = request.StudentSensitivityProfile.LightSensitivity ?? 0,
                    TemperatureSensitivity = request.StudentSensitivityProfile.TemperatureSensitivity ?? 0,
                    TouchSensitivity = request.StudentSensitivityProfile.TouchSensitivity ?? 0,
                    Distractibility = request.StudentSensitivityProfile.Distractibility ?? 0,
                    SensitiveTimeSlots = request.StudentSensitivityProfile.SensitiveTimeSlots ?? [],
                    SensitiveLocations = Student.CalculateSensitiveLocations(
                        request.StudentSensitivityProfile.SoundSensitivity ?? 0,
                        request.StudentSensitivityProfile.LightSensitivity ?? 0,
                        request.StudentSensitivityProfile.TemperatureSensitivity ?? 0,
                        request.StudentSensitivityProfile.TouchSensitivity ?? 0,
                        request.StudentSensitivityProfile.Distractibility ?? 0,
                        teachingContext.NumCols, teachingContext.NumRows, teachingContext.SeatsPerTable,
                        teachingContext.EnvironmentalAssets),
                    OverallSensitivityLevel = request.StudentSensitivityProfile.OverallSensitivityLevel ?? 0,
                    MedicalNotes = request.StudentSensitivityProfile.MedicalNotes ?? "",
                    LastUpdated = DateTimeOffset.UtcNow
                }
            ]
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
