using Application.DTO.Knowledge;
using MediatR;

namespace Application.Knowledge.Queries.GetKnowledgeArticleBySlug;

public record GetKnowledgeArticleBySlugQuery(string Slug) : IRequest<KnowledgeArticleDetailDto?>;
