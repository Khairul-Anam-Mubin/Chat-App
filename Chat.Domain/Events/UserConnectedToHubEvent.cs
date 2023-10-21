using Chat.Framework.Events;

namespace Chat.Domain.Events;

public class UserConnectedToHubEvent : IEvent
{
    public string UserId { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
}