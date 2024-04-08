using Chat.Framework.CQRS;
using Chat.Framework.EDD;
using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Events;

public class UserEmailVerifiedEvent : IEvent, IInternalMessage
{
    [Required]
    public string UserId { get; set; }
    public string? Token { get; set; }

    public UserEmailVerifiedEvent(string userId)
    {
        UserId = userId;
    }
}
