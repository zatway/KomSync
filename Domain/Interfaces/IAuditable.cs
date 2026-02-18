namespace Domain.Entities.Interfaces;

public interface IAuditable
{
    // Дата создания
    DateTime CreatedAt { get; set; }
    
    // Кто создал (ID пользователя)
    Guid CreatorId { get; set; }

    // Дата последнего изменения
    DateTime? UpdatedAt { get; set; }
    
    // Кто изменил последний раз
    Guid? LastModifiedById { get; set; }
}