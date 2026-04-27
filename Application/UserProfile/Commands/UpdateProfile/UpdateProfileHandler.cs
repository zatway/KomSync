using Application.Common.Exceptions;
using Application.DTO.UserProfile;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfile.Commands.UpdateProfile;

public class UpdateProfileHandler(
    IFmkSyncContext context,
    ICurrentUserService currentUser
) : IRequestHandler<UpdateUserProfileRequest, bool>
{
    public async Task<bool> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedAccessException("User not authorized");

        var user = await context.Users
            .Include(u => u.Department)
            .Include(u => u.Position)
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

        if (user == null)
            throw new NotFoundException("Пользователь не найден");

        // --- Аватарка ---
        if (request.AvatarFile != null)
        {
            using var ms = new MemoryStream();
            await request.AvatarFile.CopyToAsync(ms, cancellationToken);
            user.Avatar = ms.ToArray();
        }
        else if (request.idDeletedAvatar == true)
        {
            user.Avatar = null;
        }

        // --- Личные данные ---
        if (!string.IsNullOrWhiteSpace(request.FullName))
            user.FullName = request.FullName;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            // Проверка уникальности email
            var exists = await context.Users
                .AnyAsync(u => u.Email == request.Email && u.Id != user.Id, cancellationToken);
            if (exists)
                throw new ConflictException("Этот email уже занят");

            user.Email = request.Email;
        }

        if (request.DepartmentId.HasValue)
        {
            var department = await context.Departments
                .FirstOrDefaultAsync(d => d.Id == request.DepartmentId.Value, cancellationToken);
            if (department == null)
                throw new NotFoundException("Подразделение не найдено");

            user.DepartmentId = department.Id;
        }

        if (request.PositionId.HasValue)
        {
            var position = await context.Positions
                .FirstOrDefaultAsync(p => p.Id == request.PositionId.Value, cancellationToken);
            if (position == null)
                throw new NotFoundException("Должность не найдена");

            user.PositionId = position.Id;
        }

        user.UpdateTimestamp();

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}