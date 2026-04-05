using MediatR;

namespace Application.DTO.Auth;

public record ResetPasswordRequest(string Token, string NewPassword) : IRequest<bool>;
