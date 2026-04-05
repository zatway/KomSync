using Application.DTO.Analytics;
using MediatR;

namespace Application.Analytics.Queries.GetAnalyticsDashboard;

public record GetAnalyticsDashboardQuery : IRequest<AnalyticsDashboardDto>;
