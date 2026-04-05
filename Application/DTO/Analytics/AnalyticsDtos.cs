namespace Application.DTO.Analytics;

public record AnalyticsDashboardDto(
    int OpenTasks,
    int OverdueTasks,
    IReadOnlyList<StatusCountDto> TasksByStatus,
    IReadOnlyList<UserLoadDto> TopAssignees);

public record StatusCountDto(string StatusName, int Count);

public record UserLoadDto(Guid UserId, string FullName, int ActiveTaskCount);
