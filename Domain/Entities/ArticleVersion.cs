using System.Text.Json;
using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Версия статьи. Хранит состояние статьи на момент сохранения.
/// Используется для отслеживания истории изменений и восстановления.
/// </summary>
public class ArticleVersion : BaseEntity
{
    /// <summary>
    /// Идентификатор статьи, к которой относится версия.
    /// </summary>
    public Guid ArticleId { get; init; }

    /// <summary>
    /// Номер версии (например, 1, 2, 3...).
    /// </summary>
    public int VersionNumber { get; init; }

    /// <summary>
    /// Заголовок статьи на момент сохранения версии.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Содержимое статьи на момент сохранения версии (в формате JSON).
    /// </summary>
    public JsonDocument Content { get; private set; } = JsonDocument.Parse("{}");

        /// <summary>
    /// Идентификатор пользователя, который сохранил версию.
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Дата и время сохранения версии.
    /// </summary>
    public DateTimeOffset ChangedAt { get; init; }

    /// <summary>
    /// Комментарий к изменениям, внесённым в этой версии.
    /// </summary>
    public string? ChangeComment { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Ссылка на статью, к которой относится версия.
    /// </summary>
    public Article Article { get; init; } = null!;

    /// <summary>
    /// Автор версии (пользователь, который её сохранил).
    /// </summary>
    public User Author { get; private set; } = null!;
}