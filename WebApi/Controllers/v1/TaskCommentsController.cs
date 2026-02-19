using Application.DTO.Task;
using Application.DTO.TaskComments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskCommentsController(IMediator mediator) : ControllerBase
{
    // POST api/v1/Task
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddTaskCommentRequest command)
    {
        var taskId = await mediator.Send(command);
        return Ok(taskId);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateTaskCommentRequest command)
    {
        var result = await mediator.Send(command);
        return result ? Ok(result) : NotFound();
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteTaskCommentRequest command)
    {
        var result = await mediator.Send(command);
        return result ? Ok(result) : BadRequest();
    }
}