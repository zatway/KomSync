using MediatR;

namespace Application.DTO.Auth;

public record ForgotPasswordRequest(string Email) : IRequest<bool>;
