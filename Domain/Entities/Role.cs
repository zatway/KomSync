using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Определяет права доступа к различным функциям и данным.
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Тип роли, определяющий права доступа пользователя.
    /// </summary>
    public RoleType Type { get; init; }

    /// <summary>
    /// Описание роли для пояснения её назначения.
    /// </summary>
    public string? Description { get; init; }
    
    // Навигационные свойства

    /// <summary>
    /// Пользователи, которым назначена эта роль (связь многие-ко-многим).
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
}