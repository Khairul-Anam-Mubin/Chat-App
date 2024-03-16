using Chat.Application.DTOs;
using Chat.Domain.Entities;

namespace Chat.Application.Extensions;

public static class ChatModelExtension
{
    public static LatestChatModel ToLatestChatModel(this ChatModel chatModel)
    {
        return LatestChatModel.Create(
            chatModel.Id, 
            chatModel.UserId, 
            chatModel.SendTo, 
            chatModel.Message,
            chatModel.SentAt,
            chatModel.Status, 
            chatModel.IsGroupMessage);
    }

    public static ChatDto ToChatDto(this ChatModel chatModel)
    {
        return new ChatDto
        {
            Id = chatModel.Id,
            SendTo = chatModel.SendTo,
            UserId = chatModel.UserId,
            Message = chatModel.Message,
            Status = chatModel.Status,
            SentAt = chatModel.SentAt,
            IsGroupMessage = chatModel.IsGroupMessage
        };
    }
}