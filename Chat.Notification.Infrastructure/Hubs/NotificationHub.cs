using Chat.Application.Shared.Providers;
using Chat.Domain.Shared.Events;
using Chat.Framework.Extensions;
using Chat.Framework.MessageBrokers;
using Chat.Notification.Application;
using Chat.Notification.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Notification.Infrastructure.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IEventBus _eventBus;

    public NotificationHub(
        IHubConnectionService hubConnectionService,
        IEventBus eventBus)
    {
        _hubConnectionService = hubConnectionService;
        _eventBus = eventBus;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var connectionId = Context.ConnectionId;

        var userProfile = IdentityProvider.GetUserProfile(Context.User);

        if (string.IsNullOrEmpty(userProfile.Id))
        {
            return;
        }

        Console.WriteLine($"Connected UserId : {userProfile.Id}\n");

        await Groups.AddToGroupAsync(connectionId, NotificationGroupProvider.GetGroupByUserId(userProfile.Id));
        
        await _hubConnectionService.AddConnectionToHubAsync(connectionId, userProfile.Id);

        var connectedEvent = new UserConnectedToHubEvent
        {
            UserId = userProfile.Id,
            ConnectionId = connectionId
        };

        await _eventBus.PublishAsync(connectedEvent);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

        var connectionId = Context.ConnectionId;
        
        var userProfile = IdentityProvider.GetUserProfile(Context.User);

        if (string.IsNullOrEmpty(userProfile.Id))
        {
            return;
        }

        Console.WriteLine($"User Disconnected to hub...Connection Id : {connectionId}");

        Console.WriteLine($"Disconnected...UserId : {userProfile.Id}\n");

        await _hubConnectionService.RemoveConnectionFromHubAsync(connectionId);

        await Groups.RemoveFromGroupAsync(connectionId, NotificationGroupProvider.GetGroupByUserId(userProfile.Id));

        var disconnectedEvent = new UserDisconnectedToHubEvent
        {
            UserId = userProfile.Id,
            ConnectionId = connectionId
        };

        await _eventBus.PublishAsync(disconnectedEvent);
    }
}