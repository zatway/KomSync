using System.Text;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.ExportOverdueTasksCsv;

public class ExportOverdueTasksCsvHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<ExportOverdueTasksCsvQuery, byte[]>
{
    public async Task<byte[]> Handle(ExportOverdueTasksCsvQuery request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        IQueryable<Domain.Entities.Project> visible = context.Projects.Where(p => !p.IsArchived);
        if (!ProjectAccessRules.CanViewAllProjects(role))
            visible = visible.Where(p => p.OwnerId == uid || p.Members.Any(m => m.Id == uid));

        var visibleIds = await visible.Select(p => p.Id).ToListAsync(cancellationToken);
        if (visibleIds.Count == 0)
            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes("КлючЗадачи;Название;Проект;Срок;Исполнитель\n")).ToArray();

        var today = DateTime.UtcNow.Date;

        var tasks = await context.Tasks
            .AsNoTracking()
            .Include(t => t.Project)
            .Include(t => t.StatusColumn)
            .Include(t => t.Assignee)
            .Where(t => visibleIds.Contains(t.ProjectId))
            .Where(t => !t.StatusColumn.IsDoneColumn)
            .Where(t => t.Deadline != null && t.Deadline.Value.Date < today)
            .OrderBy(t => t.Deadline)
            .ToListAsync(cancellationToken);

        var sb = new StringBuilder();
        sb.AppendLine("КлючЗадачи;Название;Проект;Срок;Исполнитель");
        foreach (var t in tasks)
        {
            var key = $"{t.Project.Key}-{t.TaskNumber}";
            var assignee = t.Assignee?.FullName ?? "";
            var deadline = t.Deadline!.Value.ToString("yyyy-MM-dd");
            sb.AppendLine(
                $"{Escape(key)};{Escape(t.Title)};{Escape(t.Project.Name)};{deadline};{Escape(assignee)}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
    }

    private static string Escape(string? s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return s.Replace(';', ',');
    }
}
