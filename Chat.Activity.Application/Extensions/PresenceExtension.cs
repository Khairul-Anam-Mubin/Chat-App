using Chat.Activity.Application.DTOs;
using Chat.Activity.Domain.Entities;

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
            Status = presnece.GetUserOnlineStatus(),
            IsActive = presnece.IsActive(),
        };
    }
}