using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(500);

        // Иерархия: одна задача — много подзадач
        builder.HasOne(t => t.ParentTask)
            .WithMany(t => t.SubTasks)
            .HasForeignKey(t => t.ParentTaskId)
            .OnDelete(DeleteBehavior.Cascade); // Удаляем задачу — удаляются подзадачи

        // Связь с исполнителем
        builder.HasOne(t => t.Assignee)
            .WithMany()
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull); // Если юзер удален, задача остается "ничьей"

        builder.HasOne(t => t.Responsible)
            .WithMany()
            .HasForeignKey(t => t.ResponsibleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.StatusColumn)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.ProjectTaskStatusColumnId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
