using Application.DTO.Projects;
using Application.Interfaces;
using Application.Projects.Commands.CreateProjectTaskStatusColumn;
using Application.Projects.Commands.UploadProjectCommentAttachments;
using Application.Projects.Queries.GetProjectTaskStatusColumns;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _mediator.Send(new GetProjectsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("{id:guid}/task-status-columns")]
        public async Task<IActionResult> GetTaskStatusColumns(Guid id)
        {
            var result = await _mediator.Send(new GetProjectTaskStatusColumnsQuery(id));
            return Ok(result);
        }

        [HttpPut("{id:guid}/task-status-columns")]
        public async Task<IActionResult> CreateTaskStatusColumn(Guid id, [FromBody] CreateTaskStatusColumnBody body)
        {
            var colId = await _mediator.Send(new CreateProjectTaskStatusColumnCommand(id, body.Name, body.ColorHex));
            return Ok(colId);
        }

        public record CreateTaskStatusColumnBody(string Name, string? ColorHex);

        [HttpPut]
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
        [HttpPut("{id:guid}/comments")]
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

        [HttpPost("comments/{id:guid}/attachments")]
        public async Task<IActionResult> UploadProjectCommentAttachments([FromRoute] Guid id, [FromForm] List<IFormFile> files)
        {
            var result = await _mediator.Send(new UploadProjectCommentAttachmentsCommand(id, files));
            return Ok(result);
        }

        [HttpGet("comment-attachments/{id:guid}")]
        public async Task<IActionResult> DownloadProjectCommentAttachment(
            [FromServices] IKomSyncContext context,
            [FromServices] IFileStorage storage,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var att = await context.ProjectCommentAttachments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (att == null) return NotFound();

            var stream = await storage.OpenReadAsync(att.StoredPath, cancellationToken);
            if (stream == null) return NotFound();

            return File(stream, att.ContentType ?? "application/octet-stream", att.FileName);
        }
    }
}