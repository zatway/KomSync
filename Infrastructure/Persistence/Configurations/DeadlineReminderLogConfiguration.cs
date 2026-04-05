using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class DeadlineReminderLogConfiguration : IEntityTypeConfiguration<DeadlineReminderLog>
{
    public void Configure(EntityTypeBuilder<DeadlineReminderLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.LogKey).IsUnique();
        builder.HasOne(x => x.ProjectTask)
            .WithMany(x => x.DeadlineReminderLogs)
            .HasForeignKey(x => x.ProjectTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
