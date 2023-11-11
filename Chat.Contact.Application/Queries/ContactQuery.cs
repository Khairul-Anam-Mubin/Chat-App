using Chat.Contact.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.Contact.Application.Queries;

public class ContactQuery : PaginationQuery<ContactDto>
{
    public string UserId { get; set; } = string.Empty;
    public bool IsRequestContacts { get; set; }
    public bool IsPendingContacts { get; set; }
}