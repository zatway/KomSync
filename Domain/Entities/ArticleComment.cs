using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Комментарий к статье. Поддерживает вложенность (ответы на комментарии).
/// </summary>
public class ArticleComment : BaseEntity
{
    /// <summary>
    /// Идентификатор статьи, к которой оставлен комментарий.
    /// </summary>
    public Guid ArticleId { get; init; }

    /// <summary>
    /// Идентификатор автора комментария.
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Текст комментария.
    /// </summary>
    public string Content { get; private set; } = null!;

    /// <summary>
    /// Идентификатор родительского комментария (для вложенности). Может быть null.
    /// </summary>
    public Guid? ParentId { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Статья, к которой относится комментарий.
    /// </summary>
    public Article Article { get; private set; } = null!;

    /// <summary>
    /// Автор комментария.
    /// </summary>
    public User Author { get; private set; } = null!;

    /// <summary>
    /// Родительский комментарий (если это ответ).
    /// </summary>
    public ArticleComment? Parent { get; init; }

    /// <summary>
    /// Ответы на этот комментарий.
    /// </summary>
    public ICollection<ArticleComment> Replies { get; private set; } = new List<ArticleComment>();
}