using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ApplicationForRegistration : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public UserRole RequestedRole { get; set; } = UserRole.Employee;

    public RegistrationApplicationStatus Status { get; set; } = RegistrationApplicationStatus.Pending;

    public DateTimeOffset? ProcessedAt { get; set; }
    public Guid? ProcessedByUserId { get; set; }
}