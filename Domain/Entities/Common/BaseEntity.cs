using System;
using System.Text.Json;

namespace Domain.Entities.Common;

/// <summary>
/// Базовый класс для всех сущностей домена.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Уникальный идентификатор сущности. Генерируется автоматически при создании.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Дата и время создания сущности. Устанавливается автоматически.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Дата и время последнего обновления сущности. Обновляется при вызове UpdateTimestamp.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Обновляет временную метку UpdatedAt на текущее время.
    /// Вызывается при изменении сущности.
    /// </summary>
    public void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}