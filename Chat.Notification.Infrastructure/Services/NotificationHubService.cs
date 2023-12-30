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
}