namespace Chat.Notification.Domain.Interfaces;

public interface IHubConnectionService
{
    Task AddConnectionAsync(string connectionId, string userId);

    Task RemoveConnectionAsync(string connectionId);

    string GetConnectionId(string userId);

    List<string> GetConnectionIds(string userId);

    string GetUserId(string connectionId);

    string GetCurrentHubId();

    Task<string?> GetUserConnectedHubInstanceIdAsync(string userId);

    Task<List<string>> GetUserConnectedHubIdsAsync(string userId);

    bool IsUserConnectedWithCurrentHubInstance(string userId);

    Task<bool> IsUserConnectedWithAnyHubInstanceAsync(string userId);
}