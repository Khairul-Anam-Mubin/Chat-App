using Chat.Contacts.Application.DTOs;
using KCluster.Framework.CQRS;
using KCluster.Framework.Pagination;

namespace Chat.Contacts.Application.Queries;

public class ContactQuery : APaginationQuery<ContactDto>, IQuery
{
    public bool IsRequestContacts { get; set; }
    public bool IsPendingContacts { get; set; }
}