using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Категория для статей. Поддерживает древовидную структуру и задаёт права редактирования.
/// Используется для организации статей в иерархии.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Идентификатор родительской категории (для дерева). Может быть null.
    /// </summary>
    public Guid? ParentId { get; init; }

    /// <summary>
    /// Порядок сортировки внутри родительской категории.
    /// </summary>
    public int Order { get; private set; } = 0;

    /// <summary>
    /// Список идентификаторов ролей, которые могут редактировать эту категорию и вложенные.
    /// Хранится как массив в базе (GIN индекс).
    /// </summary>
    public List<Guid> AllowedRoleIds { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Родительская категория.
    /// </summary>
    public Category? Parent { get; private set; }

    /// <summary>
    /// Дочерние категории.
    /// </summary>
    public ICollection<Category> Children { get; private set; } = new List<Category>();

    /// <summary>
    /// Статьи, относящиеся к этой категории.
    /// </summary>
    public ICollection<Article> Articles { get; private set; } = new List<Article>();
}