using Chat.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.Application.Queries;

public class ChatListQuery : PaginationQuery<LatestChatDto>
{
    public string UserId { get; set; } = string.Empty;
}