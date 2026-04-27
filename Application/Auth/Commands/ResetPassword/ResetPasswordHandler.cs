using System.Security.Cryptography;
using System.Text;
using Application.Common.Exceptions;
using Application.DTO.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.ResetPassword;

public class ResetPasswordHandler(
    IFmkSyncContext context,
    IPasswordHasher passwordHasher)
    : IRequestHandler<ResetPasswordRequest, bool>
{
    public async Task<bool> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            throw new BadRequestException("Токен не указан");

        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
            throw new BadRequestException("Пароль должен быть не короче 6 символов");

        var hash = HashToken(request.Token.Trim());

        var token = await context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(
                t => t.TokenHash == hash && t.UsedAtUtc == null,
                cancellationToken);

        if (token == null || token.ExpiresAtUtc < DateTimeOffset.UtcNow)
            throw new BadRequestException("Недействительная или просроченная ссылка");

        token.User.PasswordHash = passwordHasher.Hash(request.NewPassword);
        token.UsedAtUtc = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string HashToken(string raw)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
