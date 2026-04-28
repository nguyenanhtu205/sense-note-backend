namespace Domain.Entities;

public class StudentSensitivityProfile
{
    public int SoundSensitivity { get; set; }

    public int LightSensitivity { get; set; }

    public int TemperatureSensitivity { get; set; }

    public int TouchSensitivity { get; set; }

    public int Distractibility { get; set; }

    public List<string> SensitiveTimeSlots { get; set; } = [];

    public List<string> SensitiveLocations { get; set; } = [];

    public List<string> Triggers { get; set; } = [];

    public Dictionary<string, int> TriggerSeverity { get; set; } = new();

    public Dictionary<string, string> TriggerToBehaviorMap { get; set; } = new();

    public List<string> PreferredInterventions { get; set; } = [];

    public int OverallSensitivityLevel { get; set; }

    public required string MedicalNotes { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}

public class Student : BaseAuditableEntity
{
    public int ClassId { get; set; }

    public required string FullName { get; set; }

    public DateTime? Birthday { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public List<StudentSensitivityProfile> StudentSensitivityProfiles { get; set; } = [];

    public Class? Class { get; set; }

    public ICollection<LessonSummary> LessonSummaries { get; private set; } = new List<LessonSummary>();

    public ICollection<SeatAssignment> SeatAssignments { get; private set; } = new List<SeatAssignment>();

    public ICollection<BehaviorLog> BehaviorLogs { get; private set; } = new List<BehaviorLog>();

    public void MarkAsAdded(int teachingContextId, string displayName)
    {
        AddDomainEvent(new StudentAddedEvent(this, teachingContextId, displayName));
    }

    public void MarkAsDeleted()
    {
        if (DeletedAt != null)
        {
            return;
        }

        DeletedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateInfo(string fullName, DateTime? birthday)
    {
        FullName = fullName;
        Birthday = birthday;
    }
}
