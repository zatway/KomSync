using MediatR;

namespace Application.Reports.Queries.ExportProjectTasksCsv;

public record ExportProjectTasksCsvQuery(Guid ProjectId) : IRequest<byte[]>;
