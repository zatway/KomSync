using Application.DTO.Projects;
using Application.DTO.Tasks;
using Application.Projects.Commands;
using Application.Projects.Commands.CreateProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskController(IMediator mediator) : ControllerBase
{
    // POST api/v1/Task
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest command)
    {
        var taskId = await mediator.Send(command);
        return Ok(taskId);
    }

    // PATCH api/v1/Task/{id}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await mediator.Send(command);
        return result ? Ok(result) : NotFound();
    }

    // POST api/v1/Task/assign
    [HttpPost("assign")] // Уникальный путь для действия
    public async Task<IActionResult> AssignUser([FromBody] AssignUserRequest command)
    {
        var result = await mediator.Send(command);
        return result ? Ok(result) : NotFound();
    }

    // POST api/v1/Task/status
    [HttpPost("status")] // Уникальный путь для действия
    public async Task<IActionResult> ChangeTaskStatus([FromBody] ChangeTaskStatusCommand command)
    {
        var result = await mediator.Send(command);
        return result ? Ok(result) : NotFound();
    }

    // DELETE api/v1/Task/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await mediator.Send(new DeleteTaskRequest(id));
        return result ? Ok(result) : NotFound();
    }

    // GET api/v1/Task/project/{projectId}
    // Изменили путь, чтобы не конфликтовать с получением одной задачи
    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<List<ProjectBriefDto>>> GetTasksList(Guid projectId)
    {
        return Ok(await mediator.Send(new GetTasksListQuery(projectId)));
    }

    // GET api/v1/Task/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskDetailedDto>> GetTaskById(Guid id)
    {
        var result = await mediator.Send(new GetTaskByIdQuery(id));
        return result != null ? Ok(result) : NotFound();
    }
}