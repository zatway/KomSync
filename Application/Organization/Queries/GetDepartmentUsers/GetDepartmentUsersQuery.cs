using Application.Organization.Dtos;
using MediatR;

namespace Application.Organization.Queries.GetDepartmentUsers;

public record GetDepartmentUsersQuery(Guid DepartmentId) : IRequest<IReadOnlyList<OrgMemberDto>>;
