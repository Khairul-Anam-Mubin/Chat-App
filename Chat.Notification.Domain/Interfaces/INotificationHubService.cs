namespace Chat.Notification.Domain.Interfaces;

public interface INotificationHubService
{
    Task SendAsync<T>(string userId, T message, string method);
    
    Task SendToUserAsync<T>(string userId, T message, string method);

    Task SendToConnectionAsync<T>(string connectionId, T message, string method);

    Task SendToGroupAsync<T>(string groupId, T message, string method);

    Task AddToGroupAsync(string groupId, string connectionId);

    Task RemoveFromGroupAsync(string groupId, string connectionId);
}