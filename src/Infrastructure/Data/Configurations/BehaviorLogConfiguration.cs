namespace Infrastructure.Data.Configurations;

public class BehaviorLogConfiguration : IEntityTypeConfiguration<BehaviorLog>
{
    public void Configure(EntityTypeBuilder<BehaviorLog> builder)
    {
        builder.ToTable("behavior_logs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.LessonId)
            .HasColumnName("lesson_id")
            .IsRequired();

        builder.Property(x => x.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(x => x.BehaviorCategoryId)
            .HasColumnName("behavior_category_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.OccurredAt)
            .HasColumnName("occurred_at")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.Antecedent)
            .HasColumnName("antecedent")
            .HasMaxLength(500);

        builder.Property(x => x.BehaviorDescription)
            .HasColumnName("behavior_description")
            .HasMaxLength(500);

        builder.Property(x => x.Consequence)
            .HasColumnName("consequence")
            .HasMaxLength(500);

        builder.Property(x => x.RawTranscription)
            .HasColumnName("raw_transcription")
            .HasMaxLength(500);

        builder.Property(x => x.SeverityLevel)
            .HasColumnName("severity_level")
            .IsRequired();

        builder.Property(x => x.AiTags)
            .HasColumnName("ai_tags")
            .HasColumnType("jsonb");

        builder.HasOne(x => x.Lesson)
            .WithMany(l => l.BehaviorLogs)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Student)
            .WithMany(s => s.BehaviorLogs)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BehaviorCategory)
            .WithMany(b => b.BehaviorLogs)
            .HasForeignKey(x => x.BehaviorCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
