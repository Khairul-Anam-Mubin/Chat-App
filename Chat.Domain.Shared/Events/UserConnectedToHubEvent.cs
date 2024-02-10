using Chat.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Events;

public class UserConnectedToHubEvent : IEvent, IInternalMessage
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string ConnectionId { get; set; } = string.Empty;
    public string? Token { get; set; }
}