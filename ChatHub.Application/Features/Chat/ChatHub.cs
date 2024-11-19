using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatHub.Application.Features.Chat;


[Authorize]
public class ChatHub : Hub
{

    private static readonly Dictionary<string, string> userConnections = new Dictionary<string, string>();

    public async Task SendMessage(int recipientUserId, string message)
    {
        if (userConnections.TryGetValue(recipientUserId.ToString(), out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }
        else
        {
            // Optionally, handle the case where the recipient is not connected
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "User is not available.");
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            userConnections[userId] = Context.ConnectionId;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId) && userConnections.ContainsKey(userId))
        {
            userConnections.Remove(userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}

