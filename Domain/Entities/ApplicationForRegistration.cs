using Domain.Entities.Common;

namespace Domain.Entities;

public class ApplicationForRegistration : BaseEntity
{
    public User User { get; set; } = new User();
}