using Chat.Activity.Application.DTOs;
using Chat.Activity.Domain.Models;
using Chat.Application.Shared.Helpers;

namespace Chat.Activity.Application.Extensions;

public static class LastSeenModelExtension
{
    public static LastSeenDto ToLastSeenDto(this LastSeenModel lastSeenModel)
    {
        return new LastSeenDto
        {
            Id = lastSeenModel.Id,
            UserId = lastSeenModel.UserId,
            LastSeenAt = lastSeenModel.LastSeenAt,
            Status = DisplayTimeHelper.GetChatListDisplayTime(lastSeenModel.LastSeenAt, "Active Now"),
            IsActive = DisplayTimeHelper.IsActive(lastSeenModel.LastSeenAt) || lastSeenModel.IsActive,
        };
    }
}