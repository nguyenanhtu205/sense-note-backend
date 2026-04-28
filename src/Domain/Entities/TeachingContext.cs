namespace Domain.Entities;

public class EnvironmentalAsset
{
    public string AssetType { get; set; } = string.Empty;

    public int X { get; set; }

    public int Y { get; set; }

    public double InfluenceRadius { get; set; }

    public string ImpactType { get; set; } = string.Empty;
}

public class TeachingContext : BaseAuditableEntity
{
    public int TeacherId { get; set; }

    public int ClassId { get; set; }

    public required string ContextName { get; set; }

    public int NumCols { get; set; }

    public int NumRows { get; set; }

    public int SeatsPerTable { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public List<EnvironmentalAsset> EnvironmentalAssets { get; set; } = [];

    public Teacher? Teacher { get; set; }

    public Class? Class { get; set; }

    public ICollection<ShareCode> ShareCodes { get; private set; } = new List<ShareCode>();

    public ICollection<Lesson> Lessons { get; private set; } = new List<Lesson>();

    public ICollection<ContextBehaviorMap> ContextBehaviorMaps { get; private set; } = new List<ContextBehaviorMap>();

    public ICollection<SeatAssignment> SeatAssignments { get; private set; } = new List<SeatAssignment>();

    public TeachingContext Clone(int teacherId, string contextName)
    {
        return new TeachingContext
        {
            TeacherId = teacherId,
            ClassId = ClassId,
            ContextName = contextName,
            NumCols = NumCols,
            NumRows = NumRows,
            SeatsPerTable = SeatsPerTable
        };
    }
}
