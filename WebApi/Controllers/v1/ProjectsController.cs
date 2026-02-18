using Application.DTO.Projects;
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
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest command)
    {
        var projectId = await mediator.Send(command);
        
        return Ok(projectId);
    }
    
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectRequest command)
    {
        var result = await mediator.Send(command);

        if (!result)
            return NotFound(new { message = "Проект не найден" });

        return NoContent(); // 204 — успешно обновлено, возвращать нечего
    }
    
    [HttpDelete("{id:guid}")] // Параметр в URL
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // Создаем запрос вручную, передавая ID из маршрута
        var result = await mediator.Send(new DeleteProjectRequest(id));

        if (!result)
            return NotFound(new { message = "Проект не найден" });

        return NoContent(); // Стандарт для DELETE — 204 No Content
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ProjectBriefDto>>> GetAll()
    {
        return Ok(await mediator.Send(new GetProjectsListQuery()));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectDetailedDto>> GetById(Guid id)
    {
        var result = await mediator.Send(new GetProjectByIdQuery(id));
    
        if (result == null) 
            return NotFound();

        return Ok(result);
    }
}