using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Domain.Models;

public class ContactModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string ContactUserId { get; set; } = string.Empty;
    public bool IsPending { get; set; }
    public DateTime CreatedAt { get; set; }
}