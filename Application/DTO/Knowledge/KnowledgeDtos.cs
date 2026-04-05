using Domain.Entities;

namespace Application.DTO.Knowledge;

public record KnowledgeArticleListItemDto(
    Guid Id,
    string Title,
    string Slug,
    Guid? ParentId,
    int SortOrder,
    DateTimeOffset? UpdatedAt,
    Guid? ProjectId,
    string? ProjectKey,
    string? ProjectName,
    Guid? ProjectTaskId,
    /// <summary>Краткий ключ задачи, напр. CRM-12.</summary>
    string? TaskDisplayKey);

public record KnowledgeArticleDetailDto(
    Guid Id,
    string Title,
    string Slug,
    string ContentMarkdown,
    Guid? ParentId,
    Guid AuthorId,
    string AuthorName,
    int SortOrder,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    Guid? ProjectId,
    string? ProjectKey,
    string? ProjectName,
    Guid? ProjectTaskId,
    string? TaskDisplayKey,
    string? TaskTitle);

public static class KnowledgeArticleDtoFactory
{
    public static string? ComputeTaskDisplayKey(KnowledgeArticle a)
    {
        if (a.LinkedTask != null && a.Project != null)
            return $"{a.Project.Key}-{a.LinkedTask.TaskNumber}";
        if (a.LinkedTask?.Project != null)
            return $"{a.LinkedTask.Project.Key}-{a.LinkedTask.TaskNumber}";
        return null;
    }

    public static KnowledgeArticleDetailDto ToDetailDto(KnowledgeArticle a)
    {
        return new KnowledgeArticleDetailDto(
            a.Id,
            a.Title,
            a.Slug,
            a.ContentMarkdown,
            a.ParentId,
            a.AuthorId,
            a.Author.FullName,
            a.SortOrder,
            a.CreatedAt,
            a.UpdatedAt,
            a.ProjectId,
            a.Project?.Key,
            a.Project?.Name,
            a.ProjectTaskId,
            ComputeTaskDisplayKey(a),
            a.LinkedTask?.Title);
    }
}
