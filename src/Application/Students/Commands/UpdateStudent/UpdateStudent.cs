namespace Application.Students.Commands.UpdateStudent;

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

public record UpdateStudentCommand(
    int StudentId,
    int TeachingContextId,
    string FullName,
    DateTime? BirthDay,
    string DisplayName,
    StudentSensitivityProfile StudentSensitivityProfile) : IRequest;

public class UpdateStudentCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateStudentCommand>
{
    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        Student? student = await context.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
        {
            throw new NotFoundException($"Student with id {request.StudentId} was not found.");
        }

        TeachingContext? teachingContext = await context.TeachingContexts
            .FirstOrDefaultAsync(tc => tc.Id == request.TeachingContextId, cancellationToken);

        if (teachingContext == null)
        {
            throw new NotFoundException($"Teaching context with id {request.TeachingContextId} was not found.");
        }

        SeatAssignment? seatAssignment = await context.SeatAssignments
            .FirstOrDefaultAsync(sa =>
                    sa.TeachingContextId == request.TeachingContextId &&
                    sa.StudentId == request.StudentId,
                cancellationToken);

        if (seatAssignment == null)
        {
            throw new NotFoundException(
                $"Seat of student with id {request.StudentId} in teaching context with id {request.TeachingContextId} was not found.");
        }

        bool basicChanged =
            student.FullName != request.FullName ||
            student.Birthday != request.BirthDay;

        if (basicChanged)
        {
            student.UpdateInfo(request.FullName, request.BirthDay);
        }

        if (seatAssignment.DisplayName != request.DisplayName)
        {
            seatAssignment.UpdateDisplayName(request.DisplayName);
        }

        List<Domain.Entities.StudentSensitivityProfile> profiles = student.StudentSensitivityProfiles;
        StudentSensitivityProfile req = request.StudentSensitivityProfile;

        Domain.Entities.StudentSensitivityProfile? last = profiles.LastOrDefault();

        bool hasChange = false;

        if (last == null)
        {
            hasChange = true;
        }

        else
        {
            if (req.SoundSensitivity.HasValue && last.SoundSensitivity != req.SoundSensitivity.Value)
            {
                hasChange = true;
            }

            if (req.LightSensitivity.HasValue && last.LightSensitivity != req.LightSensitivity.Value)
            {
                hasChange = true;
            }

            if (req.TemperatureSensitivity.HasValue && last.TemperatureSensitivity != req.TemperatureSensitivity.Value)
            {
                hasChange = true;
            }

            if (req.TouchSensitivity.HasValue && last.TouchSensitivity != req.TouchSensitivity.Value)
            {
                hasChange = true;
            }

            if (req.Distractibility.HasValue && last.Distractibility != req.Distractibility.Value)
            {
                hasChange = true;
            }

            if (req.OverallSensitivityLevel.HasValue &&
                last.OverallSensitivityLevel != req.OverallSensitivityLevel.Value)
            {
                hasChange = true;
            }

            if (req.SensitiveTimeSlots != null &&
                !last.SensitiveTimeSlots.SequenceEqual(req.SensitiveTimeSlots))
            {
                hasChange = true;
            }

            if (req.MedicalNotes != null && last.MedicalNotes != req.MedicalNotes)
            {
                hasChange = true;
            }
        }

        if (hasChange)
        {
            Domain.Entities.StudentSensitivityProfile newProfile = new()
            {
                SoundSensitivity = req.SoundSensitivity ?? last?.SoundSensitivity ?? 0,
                LightSensitivity = req.LightSensitivity ?? last?.LightSensitivity ?? 0,
                TemperatureSensitivity = req.TemperatureSensitivity ?? last?.TemperatureSensitivity ?? 0,
                TouchSensitivity = req.TouchSensitivity ?? last?.TouchSensitivity ?? 0,
                Distractibility = req.Distractibility ?? last?.Distractibility ?? 0,
                OverallSensitivityLevel = req.OverallSensitivityLevel ?? last?.OverallSensitivityLevel ?? 0,
                SensitiveTimeSlots = req.SensitiveTimeSlots ?? last?.SensitiveTimeSlots ?? [],
                SensitiveLocations = Student.CalculateSensitiveLocations(
                    request.StudentSensitivityProfile.SoundSensitivity ?? 0,
                    request.StudentSensitivityProfile.LightSensitivity ?? 0,
                    request.StudentSensitivityProfile.TemperatureSensitivity ?? 0,
                    request.StudentSensitivityProfile.TouchSensitivity ?? 0,
                    request.StudentSensitivityProfile.Distractibility ?? 0,
                    teachingContext.NumCols, teachingContext.NumRows, teachingContext.SeatsPerTable,
                    teachingContext.EnvironmentalAssets),
                MedicalNotes = req.MedicalNotes ?? last?.MedicalNotes ?? ""
            };

            profiles = profiles.TakeLast(100).ToList();

            student.StudentSensitivityProfiles = profiles.Append(newProfile).ToList();
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
