using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Events;

public class VerifiedUserAccountCreatedEvent
{
    [Required]
    public string UserId { get; set; }

    public VerifiedUserAccountCreatedEvent(string userId)
    {
        UserId = userId;
    }
}
