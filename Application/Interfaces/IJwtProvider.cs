using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtProvider
{
    public string CreateAccessToken(User user);
}