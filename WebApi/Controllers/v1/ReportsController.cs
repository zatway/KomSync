using Application.Reports.Queries.ExportOverdueTasksCsv;
using Application.Reports.Queries.ExportProjectTasksCsv;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/reports")]
[Authorize]
public class ReportsController(IMediator mediator) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/tasks.csv")]
    public async Task<IActionResult> ExportProjectTasks(Guid projectId, CancellationToken cancellationToken)
    {
        var bytes = await mediator.Send(new ExportProjectTasksCsvQuery(projectId), cancellationToken);
        return File(bytes, "text/csv; charset=utf-8", $"zadachi-proekta-{projectId}.csv");
    }

    [HttpGet("tasks/overdue.csv")]
    public async Task<IActionResult> ExportOverdue(CancellationToken cancellationToken)
    {
        var bytes = await mediator.Send(new ExportOverdueTasksCsvQuery(), cancellationToken);
        return File(bytes, "text/csv; charset=utf-8", "prosrochennye-zadachi.csv");
    }
}
