using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.Results;

namespace Chat.Activity.Domain.Entities;

public class Presence : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public DateTime LastSeenAt { get; private set; }
    public bool IsActive { get; private set; }

    private Presence(string userId) : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        IsActive = true;
        LastSeenAt = DateTime.UtcNow;
    }

    public static IResult<Presence> Create(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Error<Presence>("UserId not set");
        }

        return Result.Success(new Presence(userId));
    }
}