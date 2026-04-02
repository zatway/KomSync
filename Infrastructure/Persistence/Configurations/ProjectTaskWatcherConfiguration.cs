using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectTaskWatcherConfiguration : IEntityTypeConfiguration<ProjectTaskWatcher>
{
    public void Configure(EntityTypeBuilder<ProjectTaskWatcher> builder)
    {
        builder.HasKey(x => new { x.TaskId, x.UserId });

        builder.HasOne(x => x.Task)
            .WithMany(t => t.Watchers)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
