using Application.Organization.Dtos;
using MediatR;

namespace Application.Organization.Queries.GetPositionUsers;

public record GetPositionUsersQuery(Guid PositionId) : IRequest<IReadOnlyList<OrgMemberDto>>;
