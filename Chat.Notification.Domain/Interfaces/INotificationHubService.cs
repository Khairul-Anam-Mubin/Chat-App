namespace Chat.Notification.Domain.Interfaces;

public interface INotificationHubService
{
    Task SendAsync<T>(string userId, T message, string method);
}