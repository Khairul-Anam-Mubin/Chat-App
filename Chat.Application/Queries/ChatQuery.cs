using Chat.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.Application.Queries;

public class ChatQuery : PaginationQuery<ChatDto>
{
    public string UserId { get; set; } = string.Empty;
    public string SendTo { get; set; } = string.Empty;
}