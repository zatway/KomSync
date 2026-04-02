using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var id = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id != null)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{id}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var id = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id != null)
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{id}");
        await base.OnDisconnectedAsync(exception);
    }
}
