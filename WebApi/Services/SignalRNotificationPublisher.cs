using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services;

public class SignalRNotificationPublisher(IHubContext<NotificationHub> hub) : IRealtimeNotificationPublisher
{
    public async Task PublishToUserAsync(Guid userId, string topic, object payload, CancellationToken cancellationToken = default)
    {
        await hub.Clients.Group($"user_{userId}")
            .SendAsync("notification", new { topic, payload }, cancellationToken);
    }
}
