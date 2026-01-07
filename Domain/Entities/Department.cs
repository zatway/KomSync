using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Отдел организации. Поддерживает иерархическую структуру через самоссылку.
/// Может содержать подотделы и сотрудников.
/// </summary>
public class Department : BaseEntity
{
    /// <summary>
    /// Название отдела.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Идентификатор вышестоящего отдела (для иерархии). Может быть null.
    /// </summary>
    public Guid? ParentId { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Вышестоящий отдел (родитель).
    /// </summary>
    public Department? Parent { get; init; }

    /// <summary>
    /// Подотделы (дети).
    /// </summary>
    public ICollection<Department> Children { get; private set; } = new List<Department>();

    /// <summary>
    /// Сотрудники, входящие в этот отдел.
    /// </summary>
    public ICollection<User> Users { get; private set; } = new List<User>();
}