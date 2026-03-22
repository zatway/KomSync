using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // --- Список проектов ---
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _mediator.Send(new GetProjectsQuery());
            return Ok(result);
        }

        // --- Детали проекта ---
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(id));
            return Ok(result);
        }

        // --- Создать проект ---
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var projectId = await _mediator.Send(request);
            return Ok(projectId);
        }

        // --- Обновить проект ---
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectRequest request)
        {
            var updated = await _mediator.Send(request with { Id = id });
            return Ok(updated);
        }

        // --- Удалить проект ---
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _mediator.Send(new DeleteProjectRequest(id));
            return Ok(result);
        }

        // --- История проекта ---
        [HttpGet("{id:guid}/history")]
        public async Task<IActionResult> GetProjectHistory(Guid id)
        {
            var result = await _mediator.Send(new GetProjectHistoryQuery(id));
            return Ok(result);
        }

        // --- Комментарии ---
        [HttpPost("{id:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] CreateProjectCommentRequest request)
        {
            var comment = await _mediator.Send(request with { ProjectId = id });
            return Ok(comment);
        }

        [HttpPatch("comments/{id:guid}")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] UpdateProjectCommentRequest request)
        {
            var updated = await _mediator.Send(request with { Id = id });
            return Ok(updated);
        }

        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            var comments = await _mediator.Send(new GetProjectCommentsQuery(id));
            return Ok(comments);
        }
    }
}