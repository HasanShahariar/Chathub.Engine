using ChatHub.Application.Features.ChatRecords.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatHub.Web.Services;

public class ChatHubService : Hub
{

    private static readonly Dictionary<string, string> userConnections = new Dictionary<string, string>();

    private readonly IMediator _mediator;

    // Inject IMediator in the constructor
    public ChatHubService(IMediator mediator)
    {
        _mediator = mediator;
    }



    // Send message to a user identified by their username or any unique identifier in the JWT
    public async Task SendMessage(int receiverId , string message)
    {
        var senderId = GetUserId();

        var command = new SaveChatHistoryCommand
        {
            SenderId = int.Parse(senderId),  // Convert string to int if necessary
            ReceiverId = receiverId,
            Message = message
        };

        var result = await _mediator.Send(command);

        var senderUsername = Context.User.Identity.Name;

        // Find the recipient's connection ID using the userId
        if (userConnections.TryGetValue(receiverId .ToString(), out var connectionId))
        {
            // Send message to recipient
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderUsername, message);
        }
        else
        {
            // Optionally, handle the case where the recipient is not connected
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "Recipient not available.");
        }
    }



    // Handle when the user connects to the hub
    public override async Task OnConnectedAsync()
    {



        var userId = GetUserId();

        if (!string.IsNullOrEmpty(userId))
        {
            userConnections[userId] = Context.ConnectionId;
        }
        else
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "User is not authenticated.");
        }

        await base.OnConnectedAsync();


    }

    // Handle when the user disconnects from the hub
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  // Get user ID from JWT
        if (!string.IsNullOrEmpty(userId) && userConnections.ContainsKey(userId))
        {
            userConnections.Remove(userId);  // Remove the user from the dictionary
        }
        await base.OnDisconnectedAsync(exception);
    }

    public string GetUserId()
    {
        var token = Context.GetHttpContext()?.Request.Query["access_token"].ToString();

        //var userId = Context.User?.FindFirst("uid")?.Value;
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userId = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "uid")?.Value; 
        var UserName = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "uName")?.Value; 
        return userId;
    }
}
