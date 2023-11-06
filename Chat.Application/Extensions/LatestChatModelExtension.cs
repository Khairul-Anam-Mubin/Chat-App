using Chat.Application.DTOs;
using Chat.Application.Shared.Helpers;
using Chat.Domain.Models;

namespace Chat.Application.Extensions;

public static class LatestChatModelExtension
{
    public static LatestChatDto ToLatestChatDto(this LatestChatModel latestChatModel, string currentUserId)
    {
        return new LatestChatDto
        {
            Id = latestChatModel.Id,
            Message = latestChatModel.Message,
            Status = latestChatModel.Status,
            DurationDisplayTime = DisplayTimeHelper.GetChatListDisplayTime(latestChatModel.SentAt),
            UserId = latestChatModel.UserId == currentUserId ? latestChatModel.SendTo : latestChatModel.UserId,
            IsReceiver = latestChatModel.UserId != currentUserId,
            Occurrence = latestChatModel.UserId != currentUserId ? latestChatModel.Occurrence : 0
        };
    }
}