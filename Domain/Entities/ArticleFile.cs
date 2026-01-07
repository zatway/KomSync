namespace Domain.Entities;

/// <summary>
/// Составная сущность, представляющая связь многие-ко-многим между статьёй и файлом.
/// Используется для прикрепления файлов к статьям.
/// </summary>
public class ArticleFile
{
    /// <summary>
    /// Идентификатор статьи.
    /// </summary>
    public Guid ArticleId { get; init; }

    /// <summary>
    /// Идентификатор файла.
    /// </summary>
    public Guid FileId { get;  init; }

    // Навигационные свойства

    /// <summary>
    /// Ссылка на статью.
    /// </summary>
    public Article Article { get; private set; } = null!;

    /// <summary>
    /// Ссылка на файл.
    /// </summary>
    public FileEntity File { get; private set; } = null!;
}