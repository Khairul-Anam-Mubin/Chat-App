using Chat.Application.Interfaces;
using Chat.Application.Shared.Providers;
using Chat.Domain.Events;
using Chat.Framework.Events;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Notification.Hubs;

public class ChatHub : Hub
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IEventPublisher _eventPublisher;

    public ChatHub(
        IHubConnectionService hubConnectionService,
        IEventPublisher eventPublisher)
    {
        _hubConnectionService = hubConnectionService;
        _eventPublisher = eventPublisher;
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

        var connectedEvent = new UserConnectedToHubEvent
        {
            UserId = userProfile.Id,
            ConnectionId = connectionId
        };
        
        await _eventPublisher.PublishAsync(connectedEvent);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

        var connectionId = Context.ConnectionId;

        Console.WriteLine($"User Disconnected to hub...Connection Id : {connectionId}");

        var userId = _hubConnectionService.GetUserId(connectionId);
        
        Console.WriteLine($"Disconnected...UserId : {userId}\n");

        await _hubConnectionService.RemoveConnectionAsync(connectionId);

        if (!string.IsNullOrEmpty(userId))
        {
            var disconnectedEvent = new UserDisconnectedToHubEvent
            {
                UserId = userId,
                ConnectionId = connectionId
            };

            await _eventPublisher.PublishAsync(disconnectedEvent);
        }
    }
}