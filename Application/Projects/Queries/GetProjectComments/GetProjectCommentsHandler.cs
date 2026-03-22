using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectComments
{
    public class GetProjectCommentsHandler(IKomSyncContext context)
        : IRequestHandler<GetProjectCommentsQuery, IEnumerable<ProjectCommentDto>>
    {
        public async Task<IEnumerable<ProjectCommentDto>> Handle(GetProjectCommentsQuery request, CancellationToken cancellationToken)
        {
            // Получаем все комментарии проекта с авторами
            var comments = await context.ProjectComments
                .Include(c => c.Author)
                .Where(c => c.ProjectId == request.ProjectId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(cancellationToken);

            // Словарь для быстрого построения дерева
            var commentMap = comments.ToDictionary(
                c => c.Id,
                c => new ProjectCommentDto(
                    c.Id,
                    c.ProjectId,
                    c.Content,
                    new AuthorDto(c.AuthorId, c.Author.FullName, c.Author.Email),
                    c.CreatedAt,
                    c.UpdatedAt,
                    c.ParentId
                )
            );

            // Инициализируем Replies
            foreach (var comment in commentMap.Values)
            {
                comment.Replies = new List<ProjectCommentDto>();
            }

            // Собираем дерево
            foreach (var comment in commentMap.Values)
            {
                if (comment.ParentId.HasValue && commentMap.TryGetValue(comment.ParentId.Value, out var parent))
                {
                    parent.Replies.Add(comment);
                }
            }

            // Возвращаем только корневые комментарии
            return commentMap.Values.Where(c => !c.ParentId.HasValue).ToList();
        }
    }
}