namespace Application.DTO.Projects;
    
public record ProjectDetailedDto
{
    public ProjectDetailedDto() { } 

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string OwnerName { get; init; }
    public DateTime CreatedAt { get; init; }
} 