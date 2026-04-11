using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
{
    public void Configure(EntityTypeBuilder<TaskHistory> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.PropertyName).IsRequired().HasMaxLength(100);

        builder.HasOne(h => h.Task)
            .WithMany(t => t.History)
            .HasForeignKey(h => h.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.ChangedBy)
            .WithMany()
            .HasForeignKey(h => h.ChangedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(h => h.ChangedByDisplayName).HasMaxLength(255);
    }

}