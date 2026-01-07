namespace Domain.Entities;

/// <summary>
/// Составная сущность, представляющая связь многие-ко-многим между задачей и файлом.
/// Используется для прикрепления файлов к задачам.
/// </summary>
public class TaskFile
{
    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public Guid TaskId { get; init; }

    /// <summary>
    /// Идентификатор файла.
    /// </summary>
    public Guid FileId { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Ссылка на задачу.
    /// </summary>
    public TaskEntity Task { get; private set; } = null!;

    /// <summary>
    /// Ссылка на файл.
    /// </summary>
    public FileEntity File { get; private set; } = null!;
}