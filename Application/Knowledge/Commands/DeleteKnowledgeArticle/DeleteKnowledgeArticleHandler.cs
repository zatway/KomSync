using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Knowledge.Commands.DeleteKnowledgeArticle;

public class DeleteKnowledgeArticleHandler(
    IKomSyncContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteKnowledgeArticleCommand, bool>
{
    public async Task<bool> Handle(DeleteKnowledgeArticleCommand request, CancellationToken cancellationToken)
    {
        var role = currentUser.Role;
        if (role is not UserRole.Admin and not UserRole.Manager)
            throw new ForbiddenException("Удаление статей доступно администраторам и руководителям");

        var article = await context.KnowledgeArticles
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (article == null) return false;

        context.KnowledgeArticles.Remove(article);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
