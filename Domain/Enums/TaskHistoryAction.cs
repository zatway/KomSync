namespace Domain.Enums;

public enum TaskHistoryAction
{
    Created,        // Задача создана
    Updated,        // Задача обновлена
    StatusChanged,  // Статус изменен
    PriorityChanged,// Приоритет изменен
    AssigneeChanged,// Исполнитель изменен
}