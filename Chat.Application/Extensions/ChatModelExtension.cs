using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Application.Extensions;

public static class ChatModelExtension
{
    public static LatestChatModel ToLatestChatModel(this ChatModel chatModel)
    {
        return new LatestChatModel
        {
            Id = chatModel.Id,
            UserId = chatModel.UserId,
            SendTo = chatModel.SendTo,
            Message = chatModel.Message,
            SentAt = chatModel.SentAt,
            Status = chatModel.Status,
            IsGroupMessage = chatModel.IsGroupMessage
        };
    }

    public static ChatDto ToChatDto(this ChatModel chatModel)
    {
        return new ChatDto
        {
            Id = chatModel.Id,
            UserId = chatModel.UserId,
            Message = chatModel.Message,
            Status = chatModel.Status,
            SentAt = chatModel.SentAt,
            IsGroupMessage = chatModel.IsGroupMessage
        };
    }
}