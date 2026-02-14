using Application.Projects.Commands;
using Application.Projects.Commands.CreateProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command)
    {
        var projectId = await mediator.Send(command);
        
        return Ok(projectId);
    }
}