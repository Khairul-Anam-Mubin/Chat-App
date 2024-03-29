using Chat.Notification.Domain.Interfaces;
using Chat.Notification.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Notification.Infrastructure.Services;

public class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendToConnectionsAsync<T>(List<string> connectionIds, T message, string method)
    {
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