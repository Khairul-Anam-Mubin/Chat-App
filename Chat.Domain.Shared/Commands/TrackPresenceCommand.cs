using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Domain.Shared.Commands;

public class TrackPresenceCommand : ICommand, IInternalMessage
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Token { get ; set ; }
}