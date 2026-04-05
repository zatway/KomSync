using Application.DTO.Knowledge;
using MediatR;

namespace Application.Knowledge.Queries.GetKnowledgeArticles;

public record GetKnowledgeArticlesQuery(Guid? ProjectId = null, Guid? TaskId = null)
    : IRequest<IReadOnlyList<KnowledgeArticleListItemDto>>;
