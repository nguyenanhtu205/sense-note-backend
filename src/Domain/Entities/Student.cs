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

    public static List<string> CalculateSensitiveLocations(int soundSensitivity, int lightSensitivity,
        int temperatureSensitivity, int touchSensitivity, int distractibility, int numCols, int numRows,
        int seatsPerTable, List<EnvironmentalAsset> assets)
    {
        const double soundDistractFactor = 0.3;
        const double lightDistractFactor = 0.2;
        const double threshold = 0.6;

        double[,] heatmap = new double[(numCols * seatsPerTable) + 1, numRows + 1];
        double maxRisk = 0;

        for (int x = 1; x <= numCols * seatsPerTable; x++)
        {
            for (int y = 1; y <= numRows; y++)
            {
                double totalRisk = 0;

                foreach (EnvironmentalAsset asset in assets)
                {
                    double sensitivity = asset.ImpactType switch
                    {
                        ImpactType.Sound => soundSensitivity + (distractibility * soundDistractFactor),
                        ImpactType.Light => lightSensitivity + (distractibility * lightDistractFactor),
                        ImpactType.Distraction => distractibility,
                        ImpactType.Temperature => temperatureSensitivity,
                        ImpactType.Touch => touchSensitivity,
                        _ => 0
                    };

                    if (sensitivity <= 0)
                    {
                        continue;
                    }

                    sensitivity = Math.Clamp(sensitivity, 0, 10);

                    double dx = x - asset.X;
                    double dy = y - asset.Y;
                    double distSq = (dx * dx) + (dy * dy);

                    if (asset.InfluenceRadius <= 0)
                    {
                        continue;
                    }

                    double sigma = asset.InfluenceRadius / 2.0;
                    double sigmaSq = sigma * sigma;

                    double impact = Math.Exp(-distSq / (2 * sigmaSq));

                    double weight = asset.ImpactType switch
                    {
                        ImpactType.Sound => 1.2,
                        ImpactType.Light => 1.0,
                        ImpactType.Distraction => 1.5,
                        ImpactType.Touch => 1.0,
                        ImpactType.Temperature => 0.8,
                        _ => 1.0
                    };

                    totalRisk += sensitivity * impact * weight;
                }

                heatmap[x, y] = totalRisk;

                if (totalRisk > maxRisk)
                {
                    maxRisk = totalRisk;
                }
            }
        }

        List<string> result = [];

        if (maxRisk == 0)
        {
            return result;
        }

        for (int x = 1; x <= numCols * seatsPerTable; x++)
        {
            for (int y = 1; y <= numRows; y++)
            {
                double normalized = heatmap[x, y] / maxRisk;

                if (normalized >= threshold)
                {
                    result.Add($"({x}, {y}, {Math.Round(normalized, 2)})");
                }
            }
        }

        return result;
    }
}
