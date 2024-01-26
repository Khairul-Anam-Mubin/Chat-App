namespace Chat.Notification.Domain.Interfaces;

public interface IHubConnectionService
{
    Task AddConnectionToHubAsync(string connectionId, string userId);

    Task RemoveConnectionFromHubAsync(string connectionId);

    List<string> GetConnectionIds(string userId);

    string GetUserId(string connectionId);

    string GetCurrentHubId();

    Task<List<string>> GetUserConnectedHubIdsAsync(string userId);
}