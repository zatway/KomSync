using Application.Admin.Commands.DeleteUserAdmin;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Organization.Commands.DeletePosition;

public class DeletePositionHandler(IKomSyncContext context, ICurrentUserService currentUser, IMediator mediator)
    : IRequestHandler<DeletePositionCommand, bool>
{
    public async Task<bool> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        _ = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var pos = await context.Positions.FirstOrDefaultAsync(p => p.Id == request.PositionId, cancellationToken);
        if (pos == null) return false;

        var userIds = await context.Users
            .Where(u => u.PositionId == pos.Id)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        if (userIds.Count > 0)
        {
            if (request.DeleteAllUsers)
            {
                foreach (var uid in userIds)
                    await mediator.Send(new DeleteUserAdminCommand(uid), cancellationToken);
            }
            else if (request.ReassignToPositionId.HasValue)
            {
                if (request.ReassignToPositionId == pos.Id)
                    throw new BadRequestException("Выберите другую должность для переноса сотрудников.");

                var target = await context.Positions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == request.ReassignToPositionId.Value, cancellationToken);
                if (target == null || target.DepartmentId != pos.DepartmentId)
                    throw new BadRequestException("Целевая должность должна быть в том же подразделении.");

                await context.Users
                    .Where(u => u.PositionId == pos.Id)
                    .ExecuteUpdateAsync(
                        s => s.SetProperty(u => u.PositionId, _ => request.ReassignToPositionId!.Value),
                        cancellationToken);
            }
            else
            {
                throw new BadRequestException(
                    "На этой должности есть сотрудники: переназначьте их на другую должность или удалите всех.");
            }
        }

        context.Positions.Remove(pos);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
