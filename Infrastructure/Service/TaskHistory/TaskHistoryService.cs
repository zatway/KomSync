using Domain.Enums;

namespace Infrastructure.Service.TaskHistory;

public static class TaskHistoryService
{
    public static TaskHistoryAction DetermineTaskHistoryAction(string propertyName)
    {
        return propertyName.ToLowerInvariant() switch
        {
            "status" or "statusid" or "projecttaskstatuscolumnid" 
                => TaskHistoryAction.StatusChanged,

            "priority" or "priorityid" 
                => TaskHistoryAction.PriorityChanged,

            "assigneeid" or "assignee" 
                => TaskHistoryAction.AssigneeChanged,

            "title" or "name" 
                => TaskHistoryAction.Updated,   // можно сделать отдельный TitleChanged, если нужно

            "description" 
                => TaskHistoryAction.Updated,

            "deadline" or "duedate" or "enddate" 
                => TaskHistoryAction.Updated,

            _ => TaskHistoryAction.Updated
        };
    }
}