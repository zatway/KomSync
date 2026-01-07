using Domain.Entities;
using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Пользователь системы. Может входить через локальную регистрацию или внешние провайдеры (VK, Yandex, Sber).
/// Имеет связь с ролями, отделом, созданными и назначенными задачами, статьями и другими сущностями.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Уникальный email пользователя. Должен быть уникальным.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Нормализованный email (в верхнем регистре) для поиска без учета регистра.
    /// </summary>
    public string NormalizedEmail { get; private set; } = null!;

    /// <summary>
    /// Полное имя пользователя (ФИО).
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Должность пользователя (необязательно).
    /// </summary>
    public string? Position { get; init; }

    /// <summary>
    /// Идентификатор отдела, к которому принадлежит пользователь (необязательно).
    /// </summary>
    public Guid? DepartmentId { get; init; }

    /// <summary>
    /// Внешний провайдер аутентификации ("VK", "Yandex", "Sber") или null для локальной регистрации.
    /// </summary>
    public string? ExternalProvider { get; init; }

    /// <summary>
    /// Идентификатор пользователя во внешней системе (например, в VK).
    /// </summary>
    public string? ExternalId { get; init; }

    /// <summary>
    /// URL аватара пользователя (из соцсети или загруженного файла).
    /// </summary>
    public string? PhotoUrl { get; init; }

    /// <summary>
    /// Дата и время последнего входа пользователя в систему (необязательно).
    /// </summary>
    public DateTimeOffset? LastLoginAt { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Отдел, к которому принадлежит пользователь.
    /// </summary>
    public Department? Department { get; init; }

    /// <summary>
    /// Роли, назначенные пользователю (связь многие-ко-многим).
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// Статьи, созданные пользователем.
    /// </summary>
    public ICollection<Article> AuthoredArticles { get; private set; } = new List<Article>();

    /// <summary>
    /// Версии статей, созданные пользователем.
    /// </summary>
    public ICollection<ArticleVersion> ArticleVersions { get; private set; } = new List<ArticleVersion>();

    /// <summary>
    /// Задачи, созданные пользователем.
    /// </summary>
    public ICollection<TaskEntity> CreatedTasks { get; private set; } = new List<TaskEntity>();

    /// <summary>
    /// Задачи, назначенные пользователю.
    /// </summary>
    public ICollection<TaskEntity> AssignedTasks { get; private set; } = new List<TaskEntity>();

    /// <summary>
    /// Комментарии к задачам, оставленные пользователем.
    /// </summary>
    public ICollection<TaskComment> TaskComments { get; private set; } = new List<TaskComment>();

    /// <summary>
    /// Комментарии к статьям, оставленные пользователем.
    /// </summary>
    public ICollection<ArticleComment> ArticleComments { get; private set; } = new List<ArticleComment>();

    /// <summary>
    /// Сообщения в чате, отправленные пользователем.
    /// </summary>
    public ICollection<ChatMessage> ChatMessages { get; private set; } = new List<ChatMessage>();

    /// <summary>
    /// Файлы, загруженные пользователем.
    /// </summary>
    public ICollection<FileEntity> UploadedFiles { get; private set; } = new List<FileEntity>();
}