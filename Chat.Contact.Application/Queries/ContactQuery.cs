using System.ComponentModel.DataAnnotations;
using Chat.Contacts.Application.DTOs;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.Contacts.Application.Queries;

public class ContactQuery : APaginationQuery<ContactDto>, IQuery
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    public bool IsRequestContacts { get; set; }
    public bool IsPendingContacts { get; set; }
}