using Domain.Enums;
using MediatR;

namespace Application.DTO.Auth;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    UserRole Role,
    string DepartmentId,
    string PositionId) : IRequest;