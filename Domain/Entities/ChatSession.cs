using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Сессия чата с ИИ. Может содержать несколько сообщений.
/// Используется для поддержания контекста диалога.
/// </summary>
public class ChatSession : BaseEntity
{
    /// <summary>
    /// Идентификатор пользователя, которому принадлежит сессия.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Название сессии. Может генерироваться автоматически по первому вопросу.
    /// </summary>
    public string? Title { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Пользователь, которому принадлежит сессия.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Сообщения в этой сессии.
    /// </summary>
    public ICollection<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();
}