using Chat.Application.Interfaces;
using Chat.Application.Shared.Providers;
using Chat.Domain.Shared.Commands;
using Chat.Framework.Proxy;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Notification.Hubs;

public class ChatHub : Hub
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly ICommandQueryProxy _commandQueryProxy;

    public ChatHub(IHubConnectionService hubConnectionService, ICommandQueryProxy commandQueryProxy)
    {
        _hubConnectionService = hubConnectionService;
        _commandQueryProxy = commandQueryProxy;
    }
        
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var connectionId = Context.ConnectionId;
        var accessToken = Context?.GetHttpContext()?.Request?.Query["access_token"].ToString();

        Console.WriteLine($"User Connected With Hub....Connection Id : {connectionId}");

        if (string.IsNullOrEmpty(accessToken))
        {
            return;
        }
        
        var userProfile = IdentityProvider.GetUserProfile(accessToken);

        if (string.IsNullOrEmpty(userProfile.Id))
        {
            return;
        }

        Console.WriteLine($"Connected UserId : {userProfile.Id}\n");

        await _hubConnectionService.AddConnectionAsync(connectionId, userProfile.Id);

        await TrackLastSeenActivityAsync(userProfile.Id, true);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

        var connectionId = Context.ConnectionId;

        Console.WriteLine($"User Disconnected to hub...Connection Id : {connectionId}");

        var userId = _hubConnectionService.GetUserId(connectionId);
        
        Console.WriteLine($"Disconnected...UserId : {userId}\n");
        
        if (!string.IsNullOrEmpty(userId))
        {
            await TrackLastSeenActivityAsync(userId, false);
        }

        await _hubConnectionService.RemoveConnectionAsync(connectionId);
    }

    private async Task TrackLastSeenActivityAsync(string userId, bool isActive)
    {
        var updateLastSeenCommand = new UpdateLastSeenCommand
        {
            UserId = userId,
            ApiUrl = "https://localhost:50502/api/Activity/track",
            IsActive = isActive,
            FireAndForget = true
        };
        await _commandQueryProxy.GetCommandResponseAsync(updateLastSeenCommand);
    }
}