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
        var connectionId = Context.ConnectionId;
        var accessToken = Context?.GetHttpContext()?.Request?.Query["access_token"].ToString();

        Console.WriteLine($"Connected....Connection Id : {connectionId}");
        Console.WriteLine($"Connected....AccessToken : {accessToken}");

        if (accessToken == null) return;

        _hubConnectionService.AddConnection(connectionId, accessToken);

        var userProfile = IdentityProvider.GetUserProfile(accessToken);

        await TrackLastSeenActivityAsync(userProfile.Id, true);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        Console.WriteLine($"Disconnected...Connection Id : {connectionId}");

        var userId = _hubConnectionService.GetUserId(connectionId);
        if (!string.IsNullOrEmpty(userId))
        {
            await TrackLastSeenActivityAsync(userId, false);
        }

        _hubConnectionService.RemoveConnection(connectionId);

        await base.OnDisconnectedAsync(exception);
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