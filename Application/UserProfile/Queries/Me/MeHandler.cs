using Application.Common.Exceptions;
using Application.DTO.UserProfile;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfile.Queries.Me;

public class MeHandler(
    IFmkSyncContext context
) : IRequestHandler<MeRequest, UserResponse>
{
    public async Task<UserResponse> Handle(MeRequest request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new UserResponse(
                u.FullName,
                u.Email,
                u.Role,
                u.Department.Name,
                u.Position.Name
            ))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
            throw new NotFoundException("Пользователь не найден");

        return user;
    }
}