namespace Chat.Domain.Shared.Events;

public class UserDisconnectedToHubEvent
{
    public string UserId { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
}