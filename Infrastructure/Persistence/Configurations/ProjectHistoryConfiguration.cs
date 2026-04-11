using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectHistoryConfiguration : IEntityTypeConfiguration<ProjectHistory>
{
    public void Configure(EntityTypeBuilder<ProjectHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.HasOne(h => h.Project)
            .WithMany(p => p.ProjectHistories)
            .HasForeignKey(h => h.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.ChangedBy)
            .WithMany()
            .HasForeignKey(h => h.ChangedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(h => h.ChangedByDisplayName).HasMaxLength(255);
    }
}
