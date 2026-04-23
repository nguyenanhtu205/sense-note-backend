namespace Domain.Entities;

public class ShareCode : BaseAuditableEntity
{
    public required string Code { get; set; }

    public int SourceContextId { get; set; }

    public DateTimeOffset? ExpiredAt { get; set; }

    public TeachingContext? TeachingContext { get; set; }
}
