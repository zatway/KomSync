using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Queries.GetPendingRegistrations;

public record RegistrationApplicationAdminDto(
    Guid Id,
    Guid UserId,
    string FullName,
    string Email,
    UserRole RequestedRole,
    RegistrationApplicationStatus Status,
    DateTimeOffset CreatedAt);

public record GetPendingRegistrationsQuery : IRequest<IReadOnlyList<RegistrationApplicationAdminDto>>;

public class GetPendingRegistrationsHandler(IKomSyncContext context)
    : IRequestHandler<GetPendingRegistrationsQuery, IReadOnlyList<RegistrationApplicationAdminDto>>
{
    public async Task<IReadOnlyList<RegistrationApplicationAdminDto>> Handle(
        GetPendingRegistrationsQuery request,
        CancellationToken cancellationToken)
    {
        var list = await context.ApplicationForRegistrations
            .AsNoTracking()
            .Include(a => a.User)
            .Where(a => a.Status == RegistrationApplicationStatus.Pending)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return list.Select(a => new RegistrationApplicationAdminDto(
            a.Id,
            a.UserId,
            a.User!.FullName,
            a.User.Email,
            a.RequestedRole,
            a.Status,
            a.CreatedAt
        )).ToList();
    }
}
