using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Queries.GetAdminUsers;

public record AdminUserListItemDto(
    Guid Id,
    string FullName,
    string Email,
    UserRole Role,
    bool IsApproved);

public record GetAdminUsersQuery : IRequest<IReadOnlyList<AdminUserListItemDto>>;

public class GetAdminUsersHandler(IKomSyncContext context)
    : IRequestHandler<GetAdminUsersQuery, IReadOnlyList<AdminUserListItemDto>>
{
    public async Task<IReadOnlyList<AdminUserListItemDto>> Handle(
        GetAdminUsersQuery request,
        CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .OrderBy(u => u.FullName)
            .Select(u => new AdminUserListItemDto(u.Id, u.FullName, u.Email, u.Role, u.IsApproved))
            .ToListAsync(cancellationToken);
    }
}
