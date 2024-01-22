using Chat.Notification.Domain.Interfaces;
using Chat.Notification.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Notification.Infrastructure.Services;

public class NotificationHubService : INotificationHubService
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubService(
        IHubContext<NotificationHub> hubContext,
        IHubConnectionService hubConnectionService)
    {
        _hubConnectionService = hubConnectionService;
        _hubContext = hubContext;
    }
    [Obsolete]
    public async Task SendAsync<T>(string userId, T message, string method)
    {
        var connectionId = _hubConnectionService.GetConnectionId(userId);

        Console.WriteLine("==============Sending");

        if (string.IsNullOrEmpty(connectionId))
        {
            Console.WriteLine("ConnectionId not found");
            return;
        }
        
        await _hubContext.Clients.Client(connectionId).SendAsync(method, message);

        Console.WriteLine("==============message sent");
    }

    /// <summary>
    /// Send message to a specific user and all the connections this user holds.
    /// </summary>
    public async Task SendToUserAsync<T>(string userId, T message, string method)
    {
        var connectionIds = _hubConnectionService.GetConnectionIds(userId);
        foreach (var connectionId in connectionIds)
        {
            await SendToConnectionAsync(connectionId, message, method);
        }
    }

    public async Task SendToConnectionAsync<T>(string connectionId, T message, string method)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync(method, message);
    }

    public async Task SendToGroupAsync<T>(string groupId, T message, string method)
    {
        await _hubContext.Clients.Group(groupId).SendAsync(method, message);
    }

    public async Task AddToGroupAsync(string groupId, string connectionId)
    {
        await _hubContext.Groups.AddToGroupAsync(connectionId, groupId);
    }

    public async Task RemoveFromGroupAsync(string groupId, string connectionId)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupId);
    }
}