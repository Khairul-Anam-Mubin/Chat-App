using System.ComponentModel.DataAnnotations;
using Chat.Application.DTOs;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.Application.Queries;

public class ChatListQuery : APaginationQuery<LatestChatDto>, IQuery
{
    [Required]
    public string UserId { get; set; } = string.Empty;
}