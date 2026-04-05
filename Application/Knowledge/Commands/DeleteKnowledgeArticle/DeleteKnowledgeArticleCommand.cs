using MediatR;

namespace Application.Knowledge.Commands.DeleteKnowledgeArticle;

public record DeleteKnowledgeArticleCommand(Guid Id) : IRequest<bool>;
