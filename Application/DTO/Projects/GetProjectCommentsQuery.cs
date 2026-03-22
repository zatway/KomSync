using MediatR;

namespace Application.DTO.Projects;

public record GetProjectCommentsQuery(Guid ProjectId) : IRequest<List<ProjectCommentDto>>;
