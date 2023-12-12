using Chat.Application.DTOs;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.Application.Queries;

public class ChatQuery : APaginationQuery<ChatDto>, IQuery
{
    public string UserId { get; set; } = string.Empty;
    public string SendTo { get; set; } = string.Empty;
}