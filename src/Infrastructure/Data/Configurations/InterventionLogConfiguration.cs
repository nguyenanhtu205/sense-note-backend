namespace Infrastructure.Data.Configurations;

public class InterventionLogConfiguration : IEntityTypeConfiguration<InterventionLog>
{
    public void Configure(EntityTypeBuilder<InterventionLog> builder)
    {
        builder.ToTable("intervention_logs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.BehaviorLogId)
            .HasColumnName("behavior_log_id")
            .IsRequired();

        builder.Property(x => x.SuggestedIntervention)
            .HasColumnName("suggested_intervention")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.EffectivenessRating)
            .HasColumnName("effectiveness_rating")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}
