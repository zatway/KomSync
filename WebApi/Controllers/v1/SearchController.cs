using Application.DTO.Search;
using Application.Search.Queries.GlobalSearch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/search")]
[Authorize]
public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SearchHitDto>>> Search([FromQuery] string q, [FromQuery] int take = 40, CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(new GlobalSearchQuery(q, take), cancellationToken));
    }
}
