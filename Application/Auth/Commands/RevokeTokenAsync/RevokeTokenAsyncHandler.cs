using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Auth;

namespace Application.Auth.Commands.RevokeTokenAsync;

public class RevokeTokenAsyncHandler(
    IFmkSyncContext context
    ) : IRequestHandler<RevokeTokenRequest, bool>
{
    public async Task<bool> Handle(RevokeTokenRequest request, CancellationToken cancellationToken)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.UserId == request.UserId, cancellationToken);

        if (token == null) return false;

        context.RefreshTokens.Remove(token);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}