using Chat.Contacts.Application.DTOs;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;

namespace Chat.Contacts.Application.Queries;

public class ContactQuery : APaginationQuery<ContactDto>, IQuery<IPaginationResponse<ContactDto>>
{
    public bool IsRequestContacts { get; set; }
    public bool IsPendingContacts { get; set; }
}