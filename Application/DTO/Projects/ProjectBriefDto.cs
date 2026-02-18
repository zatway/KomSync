namespace Application.DTO.Projects;

public record ProjectBriefDto
{
    // Оставляем пустой конструктор для AutoMapper
    public ProjectBriefDto() { } 

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string OwnerName { get; init; }
    public DateTime CreatedAt { get; init; }
}