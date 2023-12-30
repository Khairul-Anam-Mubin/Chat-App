using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Events;

public class UserDisconnectedToHubEvent
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string ConnectionId { get; set; } = string.Empty;
}