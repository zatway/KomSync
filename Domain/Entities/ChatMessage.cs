using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Сообщение в чате с ИИ. Может быть от пользователя или от ИИ.
/// Хранит контекст RAG (релевантные статьи) для восстановления истории.
/// </summary>
public class ChatMessage : BaseEntity
{
    /// <summary>
    /// Идентификатор сессии, к которой относится сообщение (может быть null).
    /// </summary>
    public Guid? SessionId { get; init; }

    /// <summary>
    /// Идентификатор пользователя, отправившего сообщение (null, если от ИИ).
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public string Content { get; private set; } = null!;

    /// <summary>
    /// Флаг: сообщение от ИИ (true) или от пользователя (false).
    /// По умолчанию — false.
    /// </summary>
    public bool IsFromAI { get; init; }

    /// <summary>
    /// Список идентификаторов статей, которые были использованы в RAG-контексте при генерации ответа.
    /// Хранится для истории и анализа.
    /// </summary>
    public List<Guid> RelevantArticleIds { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Сессия, к которой относится сообщение.
    /// </summary>
    public ChatSession? Session { get; init; }

    /// <summary>
    /// Пользователь, отправивший сообщение (null, если от ИИ).
    /// </summary>
    public User? User { get; init; }
}