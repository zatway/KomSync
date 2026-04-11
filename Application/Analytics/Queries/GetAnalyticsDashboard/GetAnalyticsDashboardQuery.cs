using Application.DTO.Analytics;
using MediatR;

namespace Application.Analytics.Queries.GetAnalyticsDashboard;

public record GetAnalyticsDashboardQuery(Guid? ProjectId = null) : IRequest<AnalyticsDashboardDto>;
