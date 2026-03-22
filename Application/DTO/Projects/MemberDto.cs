using Domain.Enums;

namespace Application.DTO.Projects;
    
public record MemberDto(Guid Id, string Name, string? Email, UserRole Role);
