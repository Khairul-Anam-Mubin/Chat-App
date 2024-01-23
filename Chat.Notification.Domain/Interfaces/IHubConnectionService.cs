namespace Chat.Notification.Domain.Interfaces;

public interface IHubConnectionService
{
    Task AddConnectionAsync(string connectionId, string userId);

    Task RemoveConnectionAsync(string connectionId);

    List<string> GetConnectionIds(string userId);

    string GetUserId(string connectionId);

    string GetCurrentHubId();

    Task<List<string>> GetUserConnectedHubIdsAsync(string userId);
}