using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

/// <summary>
/// Сущность пользователя системы.
/// </summary>
[Index(nameof(Email), IsUnique = true)]
public class User
{
    /// <summary> Уникальный идентификатор пользователя. </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary> ФИО сотрудника. </summary>
    [Required, MaxLength(255)]
    public string FullName { get; set; } = null!;

    /// <summary> Уникальный Email. </summary>
    [Required, MaxLength(255)]
    public string Email { get; set; } = null!;  
    
    /// <summary> Уникальный Email. </summary>
    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    /// <summary> Роль пользователя (0=ReadOnly, 1=Employee...). </summary>
    public UserRole Role { get; set; }

    /// <summary> Должность. </summary>
    [MaxLength(100)]
    public string? Position { get; set; }

    /// <summary> Отдел. </summary>
    [MaxLength(100)]
    public string? Department { get; set; }

    /// <summary> Провайдер аутентификации (VK, Yandex). </summary>
    [MaxLength(50)]
    public string? ExternalProvider { get; set; }

    /// <summary> ID пользователя во внешней системе. </summary>
    [MaxLength(255)]
    public string? ExternalId { get; set; }

    /// <summary> Дата создания профиля. </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- Навигационные поля ---
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
}