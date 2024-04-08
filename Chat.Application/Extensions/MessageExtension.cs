using Chat.Application.DTOs;
using Chat.Domain.Entities;

namespace Chat.Application.Extensions;

public static class MessageExtension
{
    public static Conversation ToConversation(this Message message)
    {
        return Conversation.Create(
            message.Id, 
            message.SenderId, 
            message.ReceiverId, 
            message.Content,
            message.SentAt,
            message.Status, 
            message.IsGroupMessage);
    }

    public static MessageDto ToMessageDto(this Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            ReceiverId = message.ReceiverId,
            SenderId = message.SenderId,
            Content = message.Content,
            Status = message.Status,
            SentAt = message.SentAt,
            IsGroupMessage = message.IsGroupMessage
        };
    }
}