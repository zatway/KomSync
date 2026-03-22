using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IKomSyncContext
{
    DbSet<User> Users { get; }
    DbSet<ApplicationForRegistration> ApplicationForRegistrations { get; }
    DbSet<Category> Categories { get; }
    DbSet<Position> Positions { get; }
    DbSet<Department> Departments { get; }
    DbSet<ProjectComment> ProjectComments { get; }
    DbSet<ProjectHistory> ProjectHistories { get; }
    DbSet<Tag> Tags { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Project> Projects { get; }
    DbSet<ProjectTask> Tasks { get; }
    DbSet<TaskHistory> TaskHistories { get; }
    DbSet<TaskComment> TaskComments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}