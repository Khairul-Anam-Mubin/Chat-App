using Chat.Activity.Application.DTOs;
using Chat.Activity.Domain.Entities;
using Chat.Application.Shared.Helpers;

namespace Chat.Activity.Application.Extensions;

public static class PresenceExtension
{
    public static PresenceDto ToLastSeenDto(this Presence presnece)
    {
        return new PresenceDto
        {
            Id = presnece.Id,
            UserId = presnece.UserId,
            LastSeenAt = presnece.LastSeenAt,
            Status = DisplayTimeHelper.GetChatListDisplayTime(presnece.LastSeenAt, "Active Now"),
            IsActive = DisplayTimeHelper.IsActive(presnece.LastSeenAt) || presnece.IsActive,
        };
    }
}