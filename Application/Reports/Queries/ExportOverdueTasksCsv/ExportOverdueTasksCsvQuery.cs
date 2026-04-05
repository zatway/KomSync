using MediatR;

namespace Application.Reports.Queries.ExportOverdueTasksCsv;

public record ExportOverdueTasksCsvQuery : IRequest<byte[]>;
