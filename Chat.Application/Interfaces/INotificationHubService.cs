namespace Chat.Application.Interfaces;

public interface INotificationHubService
{
    Task SendAsync<T>(string userId, T message, string method = "ReceivedChat");
}