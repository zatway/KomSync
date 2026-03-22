using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Auth;

namespace Application.Auth.Commands.Register;

public class RegisterHandler(
    IKomSyncContext context,
    IPasswordHasher passwordHasher,
    IMapper mapper) : IRequestHandler<RegisterRequest>
{
    public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        // 1. Проверка уникальности
        if (await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new Exception("Пользователь с таким Email уже существует");
        
        if (await context.Departments.AnyAsync(u => u.Id.ToString() == request.DepartmentId, cancellationToken))
            throw new Exception("Подразделение не существует");      
        
        if (await context.Positions.AnyAsync(u => u.Id.ToString() == request.PositionId, cancellationToken))
            throw new Exception("Должность не существует");
        
        // 2. Создание пользователя
        var user = mapper.Map<User>(request);
        user.PasswordHash = passwordHasher.Hash(request.Password);

        await context.ApplicationForRegistrations.AddAsync(new ApplicationForRegistration() { User = user }, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}