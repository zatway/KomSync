namespace Domain.Entities;

/// <summary>
/// Составная сущность, представляющая связь многие-ко-многим между пользователем и ролью.
/// Используется для назначения ролей пользователям.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid RoleId { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Ссылка на пользователя.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Ссылка на роль.
    /// </summary>
    public Role Role { get; private set; } = null!;
}