using System.Text.Json;
using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Статья в системе знаний. Содержит заголовок, содержимое в формате JSON,
/// привязку к категории и автору. Поддерживает версионирование.
/// </summary>
public class Article : BaseEntity
{
    /// <summary>
    /// Заголовок статьи.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// URL-дружественная версия заголовка (slug) для использования в ссылках.
    /// Должна быть уникальной.
    /// </summary>
    public string Slug { get; private set; } = null!;

    /// <summary>
    /// Содержимое статьи в формате JSON (например, TipTap).
    /// </summary>
    public JsonDocument Content { get; private set; } = JsonDocument.Parse("{}");

    /// <summary>
    /// Идентификатор категории, к которой относится статья.
    /// </summary>
    public Guid CategoryId { get; init; }

    /// <summary>
    /// Идентификатор автора статьи.
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Текущая версия статьи. Инкрементируется при каждом изменении.
    /// </summary>
    public int Version { get; private set; } = 1;

    // Навигационные свойства

    /// <summary>
    /// Категория, к которой относится статья.
    /// </summary>
    public Category Category { get; private set; } = null!;

    /// <summary>
    /// Автор статьи.
    /// </summary>
    public User Author { get; private set; } = null!;

    /// <summary>
    /// Все версии статьи (история изменений).
    /// </summary>
    public ICollection<ArticleVersion> Versions { get; private set; } = new List<ArticleVersion>();

    /// <summary>
    /// Комментарии к статье.
    /// </summary>
    public ICollection<ArticleComment> Comments { get; private set; } = new List<ArticleComment>();

    /// <summary>
    /// Файлы, прикреплённые к статье.
    /// </summary>
    public ICollection<ArticleFile> ArticleFiles { get; private set; } = new List<ArticleFile>();
}