using System.ComponentModel.DataAnnotations;
using Chat.Application.DTOs;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.Application.Queries;

public class ConversationsQuery : APaginationQuery<ConversationDto>, IQuery
{
    [Required]
    public string UserId { get; set; } = string.Empty;
}