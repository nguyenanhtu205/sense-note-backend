namespace Domain.Entities;

public class InterventionLog : BaseAuditableEntity
{
    public int BehaviorLogId { get; set; }

    public required string SuggestedIntervention { get; set; }

    public int EffectivenessRating { get; set; } = 1;
    

    public BehaviorLog? BehaviorLog { get; set; }
}
