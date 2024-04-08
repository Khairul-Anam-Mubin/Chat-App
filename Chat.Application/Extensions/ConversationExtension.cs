using Chat.Application.DTOs;
using Chat.Application.Shared.Helpers;
using Chat.Domain.Entities;

namespace Chat.Application.Extensions;

public static class ConversationExtension
{
    public static ConversationDto ToConversationDto(this Conversation conversation, string currentUserId)
    {
        return new ConversationDto
        {
            Id = conversation.Id,
            Content = conversation.Content,
            Status = conversation.Status,
            DurationDisplayTime = DisplayTimeHelper.GetChatListDisplayTime(conversation.SentAt),
            UserId = conversation.SenderId == currentUserId ? conversation.ReceiverId : conversation.SenderId,
            IsReceiver = conversation.SenderId != currentUserId,
            Occurrence = conversation.SenderId != currentUserId ? conversation.Occurrence : 0,
            IsGroupConversation = conversation.IsGroupConversation
        };
    }
}