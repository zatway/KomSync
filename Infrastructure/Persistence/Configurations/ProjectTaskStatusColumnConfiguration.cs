using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectTaskStatusColumnConfiguration : IEntityTypeConfiguration<ProjectTaskStatusColumn>
{
    public void Configure(EntityTypeBuilder<ProjectTaskStatusColumn> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.HasIndex(x => new { x.ProjectId, x.SortOrder });

        builder.HasOne(x => x.Project)
            .WithMany(p => p.TaskStatusColumns)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
