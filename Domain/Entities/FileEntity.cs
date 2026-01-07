using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Файл, загруженный в систему. Хранится в MinIO или другом хранилище.
/// Используется как вложение к статьям и задачам.
/// </summary>
public class FileEntity : BaseEntity
{
    /// <summary>
    /// Оригинальное имя файла.
    /// </summary>
    public string FileName { get; private set; } = null!;

    /// <summary>
    /// Уникальное имя файла в хранилище (например, MinIO).
    /// </summary>
    public string StoredName { get; private set; } = null!;

    /// <summary>
    /// MIME-тип файла (например, "image/png").
    /// </summary>
    public string MimeType { get; private set; } = null!;

    /// <summary>
    /// Размер файла в байтах.
    /// </summary>
    public long Size { get; init; }

    /// <summary>
    /// Идентификатор пользователя, который загрузил файл.
    /// </summary>
    public Guid UploadedById { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Пользователь, загрузивший файл.
    /// </summary>
    public User UploadedBy { get; private set; } = null!;

    /// <summary>
    /// Связи с статьями, к которым прикреплён файл.
    /// </summary>
    public ICollection<ArticleFile> ArticleFiles { get; private set; } = new List<ArticleFile>();

    /// <summary>
    /// Связи с задачами, к которым прикреплён файл.
    /// </summary>
    public ICollection<TaskFile> TaskFiles { get; private set; } = new List<TaskFile>();
}