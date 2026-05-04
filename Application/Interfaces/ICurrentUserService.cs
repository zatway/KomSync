using Domain.Enums;

namespace Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    UserRole? Role { get; }
    /// <summary>Отдел пользователя (из JWT). Для сотрудника/читателя — фильтр видимости проектов.</summary>
    Guid? DepartmentId { get; }
}