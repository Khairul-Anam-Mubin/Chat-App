namespace Chat.Notification.Domain.Interfaces;

public interface INotificationHubService
{
    /// <summary>
    /// Send message to a specific user and all the connections this user holds.
    /// </summary>
    Task SendToUserAsync<T>(string userId, T message, string method);

    Task SendToConnectionAsync<T>(string connectionId, T message, string method);

    Task SendToGroupAsync<T>(string groupId, T message, string method);

    Task AddToGroupAsync(string groupId, string connectionId);

    Task RemoveFromGroupAsync(string groupId, string connectionId);
}