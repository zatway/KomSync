namespace Application.DTO.Projects;

public record TaskStatsDto(int Total, int Open, int InProgress, int Review, int Done, int Blocked, int Cancelled);