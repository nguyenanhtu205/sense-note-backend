namespace Domain.Entities;

public class BehaviorLog : BaseAuditableEntity
{
    public int LessonId { get; set; }

    public int StudentId { get; set; }

    public int BehaviorCategoryId { get; set; }

    public DateTimeOffset OccurredAt { get; set; }

    public string? Antecedent { get; set; }

    public string? BehaviorDescription { get; set; }

    public string? Consequence { get; set; }

    public string? RawTranscription { get; set; }

    public int SeverityLevel { get; set; } = 1;

    public List<string> AiTags { get; set; } = [];

    public Lesson? Lesson { get; set; }

    public Student? Student { get; set; }

    public BehaviorCategory? BehaviorCategory { get; set; }

    public ICollection<InterventionLog> InterventionLogs { get; set; } = new List<InterventionLog>();
}
