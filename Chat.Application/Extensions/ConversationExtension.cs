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
            UserId = conversation.UserId == currentUserId ? conversation.SendTo : conversation.UserId,
            IsReceiver = conversation.UserId != currentUserId,
            Occurrence = conversation.UserId != currentUserId ? conversation.Occurrence : 0,
            IsGroupMessage = conversation.IsGroupMessage
        };
    }
}