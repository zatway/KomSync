using Application.Analytics.Queries.GetAnalyticsDashboard;
using Application.DTO.Analytics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/analytics")]
[Authorize]
public class AnalyticsController(IMediator mediator) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<AnalyticsDashboardDto>> Dashboard([FromQuery] Guid? projectId, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetAnalyticsDashboardQuery(projectId), cancellationToken));
    }
}
