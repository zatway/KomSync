using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/positions")]
public class PositionsController(IKomSyncContext context) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? departmentId, CancellationToken cancellationToken)
    {
        var q = context.Positions.AsNoTracking();
        if (departmentId.HasValue)
            q = q.Where(p => p.DepartmentId == departmentId.Value);

        var items = await q
            .OrderBy(p => p.Name)
            .Select(p => new { id = p.Id, name = p.Name, departmentId = p.DepartmentId })
            .ToListAsync(cancellationToken);

        return Ok(items);
    }
}

