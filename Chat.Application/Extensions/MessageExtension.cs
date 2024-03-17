using Chat.Application.DTOs;
using Chat.Domain.Entities;

namespace Chat.Application.Extensions;

public static class MessageExtension
{
    public static Conversation ToConversation(this Message message)
    {
        return Conversation.Create(
            message.Id, 
            message.UserId, 
            message.SendTo, 
            message.Content,
            message.SentAt,
            message.Status, 
            message.IsGroupMessage);
    }

    public static MessageDto ToMessageDto(this Message chat)
    {
        return new MessageDto
        {
            Id = chat.Id,
            SendTo = chat.SendTo,
            UserId = chat.UserId,
            Content = chat.Content,
            Status = chat.Status,
            SentAt = chat.SentAt,
            IsGroupMessage = chat.IsGroupMessage
        };
    }
}