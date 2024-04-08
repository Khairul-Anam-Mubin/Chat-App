using Chat.Activity.Domain.Results;
using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Activity.Domain.Entities;

public class Presence : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public DateTime LastSeenAt { get; private set; }

    private Presence(string userId) : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        LastSeenAt = DateTime.UtcNow;
    }

    public static IResult<Presence> Create(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Error<Presence>().IdNotSet();
        }

        return Result.Success(new Presence(userId));
    }

    public (int minutes, int hours, int days) GetLastSeenTimeDifferences()
    {
        var timeDifference = DateTime.UtcNow.Subtract(LastSeenAt);

        var minutes = (int)timeDifference.TotalMinutes;
        var hours = (int)timeDifference.TotalHours;
        var days = (int)timeDifference.TotalDays;

        return (minutes, hours, days);
    }

    public bool IsActive()
    {
        var (minutes, hours, days) = GetLastSeenTimeDifferences();

        return days == 0 && hours == 0 && minutes == 0;
    }

    public string GetUserOnlineStatus()
    {
        if (IsActive())
        {
            return "Active now";
        }

        var (minutes, hours, days) = GetLastSeenTimeDifferences();

        if (days == 0 && hours == 0)
        {
            return $"{minutes} {GetSuffixText(minutes, "minute")} ago";
        }

        if (days == 0)
        {
            return $"{hours} {GetSuffixText(hours, "hour")} ago";
        }

        return $"{days} {GetSuffixText(days, "day")} ago";
    }

    private string GetSuffixText(int num, string text)
    {
        return num > 1 ? text + "s" : text;
    }
}