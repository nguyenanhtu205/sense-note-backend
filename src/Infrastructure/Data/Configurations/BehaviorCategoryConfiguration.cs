namespace Infrastructure.Data.Configurations;

public class BehaviorCategoryConfiguration : IEntityTypeConfiguration<BehaviorCategory>
{
    public void Configure(EntityTypeBuilder<BehaviorCategory> builder)
    {
        builder.ToTable("behavior_categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.TeacherId)
            .HasColumnName("teacher_id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PointValue)
            .HasColumnName("point_value")
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestamptz");

        builder.Property(x => x.IsAntecedent)
            .HasColumnName("is_antecedent")
            .HasDefaultValue(false);

        builder.Property(x => x.IsBehavior)
            .HasColumnName("is_behavior")
            .HasDefaultValue(true);

        builder.Property(x => x.IsConsequence)
            .HasColumnName("is_consequence")
            .HasDefaultValue(false);

        builder.Property(x => x.LabelGroup)
            .HasColumnName("label_group")
            .HasMaxLength(100);

        builder.HasOne(x => x.Teacher)
            .WithMany(t => t.BehaviorCategories)
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
