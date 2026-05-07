namespace Application.Students.Commands.UpdateStudent;

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

public record UpdateStudentCommand(
    int StudentId,
    int TeachingContextId,
    string FullName,
    DateTime? BirthDay,
    string DisplayName,
    StudentSensitivityProfile StudentSensitivityProfile) : IRequest<List<string>>;

public class UpdateStudentCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateStudentCommand, List<string>>
{
    public async Task<List<string>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
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
            if (last.SoundSensitivity != req.SoundSensitivity)
            {
                hasChange = true;
            }

            if (last.LightSensitivity != req.LightSensitivity)
            {
                hasChange = true;
            }

            if (last.TemperatureSensitivity != req.TemperatureSensitivity)
            {
                hasChange = true;
            }

            if (last.TouchSensitivity != req.TouchSensitivity)
            {
                hasChange = true;
            }

            if (last.Distractibility != req.Distractibility)
            {
                hasChange = true;
            }

            if (last.OverallSensitivityLevel != req.OverallSensitivityLevel)
            {
                hasChange = true;
            }

            if (req.SensitiveTimeSlots != null && !last.SensitiveTimeSlots.SequenceEqual(req.SensitiveTimeSlots))
            {
                hasChange = true;
            }

            if (last.MedicalNotes != req.MedicalNotes)
            {
                hasChange = true;
            }
        }

        List<string> sensitiveLocations = hasChange
            ? Student.CalculateSensitiveLocations(
                request.StudentSensitivityProfile.SoundSensitivity,
                request.StudentSensitivityProfile.LightSensitivity,
                request.StudentSensitivityProfile.TemperatureSensitivity,
                request.StudentSensitivityProfile.TouchSensitivity,
                request.StudentSensitivityProfile.Distractibility,
                teachingContext.NumCols, teachingContext.NumRows, teachingContext.SeatsPerTable,
                teachingContext.EnvironmentalAssets)
            : last?.SensitiveLocations ?? [];

        if (hasChange)
        {
            Domain.Entities.StudentSensitivityProfile newProfile = new()
            {
                SoundSensitivity = req.SoundSensitivity,
                LightSensitivity = req.LightSensitivity,
                TemperatureSensitivity = req.TemperatureSensitivity,
                TouchSensitivity = req.TouchSensitivity,
                Distractibility = req.Distractibility,
                OverallSensitivityLevel = req.OverallSensitivityLevel,
                SensitiveTimeSlots = req.SensitiveTimeSlots ?? last?.SensitiveTimeSlots ?? [],
                SensitiveLocations = sensitiveLocations,
                MedicalNotes = req.MedicalNotes
            };

            profiles = profiles.TakeLast(99).ToList();

            student.StudentSensitivityProfiles = profiles.Append(newProfile).ToList();
        }

        await context.SaveChangesAsync(cancellationToken);

        return sensitiveLocations;
    }
}
