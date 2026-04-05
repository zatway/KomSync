using System.Text;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.ExportProjectTasksCsv;

public class ExportProjectTasksCsvHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<ExportProjectTasksCsvQuery, byte[]>
{
    public async Task<byte[]> Handle(ExportProjectTasksCsvQuery request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var project = await context.Projects
            .Include(p => p.Members)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException("Проект не найден");

        if (!ProjectAccessRules.UserCanViewProject(role, uid, project))
            throw new ForbiddenException("Нет доступа к проекту");

        var tasks = await context.Tasks
            .AsNoTracking()
            .Include(t => t.StatusColumn)
            .Include(t => t.Assignee)
            .Where(t => t.ProjectId == project.Id)
            .OrderBy(t => t.TaskNumber)
            .ToListAsync(cancellationToken);

        var sb = new StringBuilder();
        sb.AppendLine("TaskKey;Title;Status;Priority;Deadline;Assignee");
        foreach (var t in tasks)
        {
            var key = $"{project.Key}-{t.TaskNumber}";
            var assignee = t.Assignee?.FullName ?? "";
            var deadline = t.Deadline?.ToString("yyyy-MM-dd") ?? "";
            sb.AppendLine(
                $"{Escape(key)};{Escape(t.Title)};{Escape(t.StatusColumn.Name)};{t.Priority};{deadline};{Escape(assignee)}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
    }

    private static string Escape(string? s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return s.Replace(';', ',');
    }
}
