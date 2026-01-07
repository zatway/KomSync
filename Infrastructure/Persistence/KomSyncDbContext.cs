using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// Основной DbContext для приложения KomSync.
/// Содержит DbSet для всех сущностей и настройку индексов, связей и конвенций.
/// </summary>
public class KomSyncDbContext(DbContextOptions<KomSyncDbContext> options) : DbContext(options)
{
    // DbSets
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
    public virtual DbSet<Department> Departments { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Article> Articles { get; set; } = null!;
    public virtual DbSet<ArticleVersion> ArticleVersions { get; set; } = null!;
    public virtual DbSet<ArticleComment> ArticleComments { get; set; } = null!;
    public virtual DbSet<TaskEntity> Tasks { get; set; } = null!;
    public virtual DbSet<TaskComment> TaskComments { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;
    public virtual DbSet<TaskTag> TaskTags { get; set; } = null!;
    public virtual DbSet<FileEntity> Files { get; set; } = null!;
    public virtual DbSet<ArticleFile> ArticleFiles { get; set; } = null!;
    public virtual DbSet<TaskFile> TaskFiles { get; set; } = null!;
    public virtual DbSet<ChatSession> ChatSessions { get; set; } = null!;
    public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;

    /// <summary>
    /// Настраивает модель, индексы, связи и конфигурацию для PostgreSQL через EF Core.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KomSyncDbContext).Assembly);

        ConfigureUserEntity(modelBuilder);
        ConfigureUserRoleEntity(modelBuilder);
        ConfigureRoleEntity(modelBuilder);
        ConfigureDepartmentEntity(modelBuilder);
        ConfigureCategoryEntity(modelBuilder);
        ConfigureArticleEntity(modelBuilder);
        ConfigureArticleVersionEntity(modelBuilder);
        ConfigureArticleCommentEntity(modelBuilder);
        ConfigureTaskEntity(modelBuilder);
        ConfigureTaskCommentEntity(modelBuilder);
        ConfigureTagEntity(modelBuilder);
        ConfigureTaskTagEntity(modelBuilder);
        ConfigureFileEntity(modelBuilder);
        ConfigureArticleFileEntity(modelBuilder);
        ConfigureTaskFileEntity(modelBuilder);
        ConfigureChatSessionEntity(modelBuilder);
        ConfigureChatMessageEntity(modelBuilder);
    }

    private static void ConfigureUserEntity(ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.NormalizedEmail);
            entity.HasIndex(e => new { e.ExternalProvider, e.ExternalId }).IsUnique();
            entity.HasIndex(e => e.CreatedAt);

            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.ExternalProvider).HasMaxLength(50);
            entity.Property(e => e.ExternalId).HasMaxLength(100);
            entity.Property(e => e.PhotoUrl);

            entity.HasOne(d => d.Department)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId);

            entity.HasMany(d => d.UserRoles)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            entity.HasMany(d => d.AuthoredArticles)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);

            entity.HasMany(d => d.ArticleVersions)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);

            entity.HasMany(d => d.CreatedTasks)
                .WithOne(p => p.Creator)
                .HasForeignKey(p => p.CreatorId);

            entity.HasMany(d => d.AssignedTasks)
                .WithOne(p => p.Assignee)
                .HasForeignKey(p => p.AssigneeId);

            entity.HasMany(d => d.TaskComments)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);

            entity.HasMany(d => d.ArticleComments)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);

            entity.HasMany(d => d.ChatMessages)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            entity.HasMany(d => d.UploadedFiles)
                .WithOne(p => p.UploadedBy)
                .HasForeignKey(p => p.UploadedById);
        });
    }

    private static void ConfigureUserRoleEntity(ModelBuilder builder)
    {
        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId);
        });
    }

    private static void ConfigureRoleEntity(ModelBuilder builder)
    {
        builder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Type).IsUnique();

            entity.Property(e => e.Description);
        });
    }

    private static void ConfigureDepartmentEntity(ModelBuilder builder)
    {
        builder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId);
        });
    }

    private static void ConfigureCategoryEntity(ModelBuilder builder)
    {
        builder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId);

            entity.HasIndex(e => e.ParentId);
            entity.HasIndex(e => new { e.ParentId, e.Order });

            entity.HasIndex(e => e.AllowedRoleIds)
                .HasDatabaseName("IX_Categories_AllowedRoleIds")
                .HasMethod("gin");
        });
    }

    private static void ConfigureArticleEntity(ModelBuilder builder)
    {
        builder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.UpdatedAt);

            entity.Property(e => e.Title).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Slug).HasMaxLength(300).IsRequired();

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.Author)
                .WithMany(p => p.AuthoredArticles)
                .HasForeignKey(d => d.AuthorId);

            entity.HasMany(d => d.Versions)
                .WithOne(p => p.Article)
                .HasForeignKey(p => p.ArticleId);

            entity.HasMany(d => d.Comments)
                .WithOne(p => p.Article)
                .HasForeignKey(p => p.ArticleId);

            entity.HasMany(d => d.ArticleFiles)
                .WithOne(p => p.Article)
                .HasForeignKey(p => p.ArticleId);
        });
    }

    private static void ConfigureArticleVersionEntity(ModelBuilder builder)
    {
        builder.Entity<ArticleVersion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ArticleId);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.ChangedAt);

            entity.Property(e => e.Title).HasMaxLength(300).IsRequired();

            entity.HasOne(d => d.Article)
                .WithMany(p => p.Versions)
                .HasForeignKey(d => d.ArticleId);

            entity.HasOne(d => d.Author)
                .WithMany(p => p.ArticleVersions)
                .HasForeignKey(d => d.AuthorId);
        });
    }

    private static void ConfigureArticleCommentEntity(ModelBuilder builder)
    {
        builder.Entity<ArticleComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ArticleId);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.ParentId);

            entity.HasOne(d => d.Article)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArticleId);

            entity.HasOne(d => d.Author)
                .WithMany(p => p.ArticleComments)
                .HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.Replies)
                .HasForeignKey(d => d.ParentId);
        });
    }

    private static void ConfigureTaskEntity(ModelBuilder builder)
    {
        builder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AssigneeId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.Deadline);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.UpdatedAt);

            entity.Property(e => e.Title).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Description);
            // Status и Priority теперь enum, не требуют MaxLength

            entity.HasOne(d => d.Creator)
                .WithMany(p => p.CreatedTasks)
                .HasForeignKey(d => d.CreatorId);

            entity.HasOne(d => d.Assignee)
                .WithMany(p => p.AssignedTasks)
                .HasForeignKey(d => d.AssigneeId);

            entity.HasOne(d => d.ParentTask)
                .WithMany(p => p.Subtasks)
                .HasForeignKey(d => d.ParentTaskId);

            entity.HasMany(d => d.Comments)
                .WithOne(p => p.Task)
                .HasForeignKey(p => p.TaskId);

            entity.HasMany(d => d.TaskTags)
                .WithOne(p => p.Task)
                .HasForeignKey(p => p.TaskId);

            entity.HasMany(d => d.TaskFiles)
                .WithOne(p => p.Task)
                .HasForeignKey(p => p.TaskId);
        });
    }

    private static void ConfigureTaskCommentEntity(ModelBuilder builder)
    {
        builder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TaskId);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(d => d.Task)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.TaskId);

            entity.HasOne(d => d.Author)
                .WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.AuthorId);
        });
    }

    private static void ConfigureTagEntity(ModelBuilder builder)
    {
        builder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Color).HasMaxLength(7);

            entity.HasMany(d => d.TaskTags)
                .WithOne(p => p.Tag)
                .HasForeignKey(p => p.TagId);
        });
    }

    private static void ConfigureTaskTagEntity(ModelBuilder builder)
    {
        builder.Entity<TaskTag>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.TagId });

            entity.HasOne(d => d.Task)
                .WithMany(p => p.TaskTags)
                .HasForeignKey(d => d.TaskId);

            entity.HasOne(d => d.Tag)
                .WithMany(p => p.TaskTags)
                .HasForeignKey(d => d.TagId);
        });
    }

    private static void ConfigureFileEntity(ModelBuilder builder)
    {
        builder.Entity<FileEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FileName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.StoredName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.MimeType).HasMaxLength(100).IsRequired();

            entity.HasOne(d => d.UploadedBy)
                .WithMany(p => p.UploadedFiles)
                .HasForeignKey(d => d.UploadedById);

            entity.HasMany(d => d.ArticleFiles)
                .WithOne(p => p.File)
                .HasForeignKey(p => p.FileId);

            entity.HasMany(d => d.TaskFiles)
                .WithOne(p => p.File)
                .HasForeignKey(p => p.FileId);
        });
    }

    private static void ConfigureArticleFileEntity(ModelBuilder builder)
    {
        builder.Entity<ArticleFile>(entity =>
        {
            entity.HasKey(e => new { e.ArticleId, e.FileId });

            entity.HasOne(d => d.Article)
                .WithMany(p => p.ArticleFiles)
                .HasForeignKey(d => d.ArticleId);

            entity.HasOne(d => d.File)
                .WithMany(p => p.ArticleFiles)
                .HasForeignKey(d => d.FileId);
        });
    }

    private static void ConfigureTaskFileEntity(ModelBuilder builder)
    {
        builder.Entity<TaskFile>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.FileId });

            entity.HasOne(d => d.Task)
                .WithMany(p => p.TaskFiles)
                .HasForeignKey(d => d.TaskId);

            entity.HasOne(d => d.File)
                .WithMany(p => p.TaskFiles)
                .HasForeignKey(d => d.FileId);
        });
    }

    private static void ConfigureChatSessionEntity(ModelBuilder builder)
    {
        builder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasMany(d => d.Messages)
                .WithOne(p => p.Session)
                .HasForeignKey(p => p.SessionId);
        });
    }

    private static void ConfigureChatMessageEntity(ModelBuilder builder)
    {
        builder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsFromAI);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.RelevantArticleIds)
                .HasDatabaseName("IX_ChatMessages_RelevantArticleIds")
                .HasMethod("gin");

            entity.HasOne(d => d.Session)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.SessionId);

            entity.HasOne(d => d.User)
                .WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.UserId);
        });
    }
}