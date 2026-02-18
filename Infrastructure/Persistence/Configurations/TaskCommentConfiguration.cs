using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Content).IsRequired().HasMaxLength(500);

        builder.HasOne(h => h.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(h => h.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}