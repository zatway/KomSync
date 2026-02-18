namespace Domain.Enums;

public enum TaskHistoryAction
{
    Created,        // Задача создана
    Updated,        // Задача обновлена
    StatusChanged,  // Статус изменен
    PriorityChanged,// Приоритет изменен
    AssigneeChanged,// Исполнитель изменен
    Deleted,        // Задача удалена (если используется Soft Delete)
    CommentAdded    // Добавлен комментарий
}