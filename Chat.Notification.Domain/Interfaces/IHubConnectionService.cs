namespace Chat.Notification.Domain.Interfaces;

public interface IHubConnectionService
{
    Task AddConnectionAsync(string connectionId, string userId);

    Task RemoveConnectionAsync(string connectionId);

    string GetConnectionId(string userId);

    string GetUserId(string connectionId);

    string GetCurrentHubInstanceId();

    Task<string?> GetUserConnectedHubInstanceIdAsync(string userId);

    bool IsUserConnectedWithCurrentHubInstance(string userId);

    Task<bool> IsUserConnectedWithAnyHubInstanceAsync(string userId);
}