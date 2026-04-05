using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

/// <summary>
/// Сущность пользователя системы.
/// </summary>
[Index(nameof(NormalizedEmail), IsUnique = true)]
public class User : BaseEntity
{
    /// <summary> Аватар сотрудника. </summary>
    public byte[]? Avatar { get; set; }
    
    /// <summary> ФИО сотрудника. </summary>
    [Required, MaxLength(255)]
    public string FullName { get; set; } = null!;

    /// <summary> Уникальный Email. </summary>
    [Required, MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string NormalizedEmail { get; set; } = null!;
    
    /// <summary> Уникальный Email. </summary>
    [Required]
    public string PasswordHash { get; set; } = null!;

    /// <summary> Роль пользователя </summary>
    public UserRole Role { get; set; } = UserRole.Employee;

    /// <summary> Доступ в систему после одобрения заявки (кроме сид-админа).</summary>
    public bool IsApproved { get; set; }
    
    /// <summary> Должность. </summary>
    [Required]
    public Guid PositionId { get; set; }
    [ForeignKey(nameof(PositionId))] 
    public Position Position { get; set; } = null!;

    /// <summary> Отдел. </summary>
    [Required]
    public Guid DepartmentId { get; set; }
    [ForeignKey(nameof(DepartmentId))] 
    public Department Department { get; set; } = null!;

    // --- Навигационные поля ---
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    public ICollection<ProjectTask> OwnedTasks { get; set; } = new List<ProjectTask>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<KnowledgeArticle> AuthoredKnowledgeArticles { get; set; } = new List<KnowledgeArticle>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
}