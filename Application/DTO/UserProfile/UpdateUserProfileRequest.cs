using Microsoft.AspNetCore.Http;
using Domain.Enums;
using MediatR;

namespace Application.DTO.UserProfile;

public record UpdateUserProfileRequest(
    IFormFile? AvatarFile,
    string? FullName,
    string? Email,
    bool? idDeletedAvatar,
    Guid? DepartmentId,
    Guid? PositionId
) : IRequest<bool>;