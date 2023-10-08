using Chat.Application.Interfaces;
using Chat.Framework.Attributes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Hubs;

[ServiceRegister(typeof(IChatHubService), ServiceLifetime.Singleton)]
public class ChatHubService : IChatHubService
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatHubService(IHubContext<ChatHub> hubContext, IHubConnectionService hubConnectionService)
    {
        _hubConnectionService = hubConnectionService;
        _hubContext = hubContext;
    }

    public async Task SendAsync<T>(string userId, T message, string method = "ReceivedChat")
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