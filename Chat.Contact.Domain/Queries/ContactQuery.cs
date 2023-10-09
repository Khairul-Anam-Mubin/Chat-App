using Chat.Framework.CQRS;

namespace Chat.Contact.Domain.Queries;

public class ContactQuery : AQuery
{
    public string UserId { get; set; } = string.Empty;
    public bool IsRequestContacts { get; set; }
    public bool IsPendingContacts { get; set; }
}