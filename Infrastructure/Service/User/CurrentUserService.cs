using System.Security.Claims;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Service.User;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var idClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(idClaim, out var parsedGuid) 
                ? parsedGuid 
                : null;
        }
    }

    public UserRole? Role
    {
        get
        {
            var r = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
            return Enum.TryParse<UserRole>(r, out var role) ? role : null;
        }
    }

    public Guid? DepartmentId
    {
        get
        {
            var raw = httpContextAccessor.HttpContext?.User?.FindFirst("departmentId")?.Value;
            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }
}