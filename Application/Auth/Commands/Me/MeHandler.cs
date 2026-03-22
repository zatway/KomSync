using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Auth;

namespace Application.Auth.Commands.Me;

public class MeHandler(
    IKomSyncContext context
) : IRequestHandler<MeRequest, UserResponse>
{
    public async Task<UserResponse> Handle(MeRequest request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid user");

        return new UserResponse(
            user.Avatar,
            user.FullName,
            user.Email,
            user.Role,
            user.Department.Name,
            user.Position.Name
        );
    }
}