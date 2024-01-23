namespace Chat.Notification.Domain.Interfaces;

public interface INotificationHubService
{
    Task SendToConnectionAsync<T>(string connectionId, T message, string method);

    Task SendToConnectionsAsync<T>(List<string> connectionIds, T message, string method);

    Task SendToGroupAsync<T>(string groupId, T message, string method);

    Task AddToGroupAsync(string groupId, string connectionId);

    Task RemoveFromGroupAsync(string groupId, string connectionId);
}