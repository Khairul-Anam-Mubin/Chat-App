using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Activity.Domain.Entities;

public class LastSeenModel : IEntity
{
    public string Id { get; set; }
    public string UserId { get; private set; }
    public DateTime LastSeenAt { get; private set; }
    public bool IsActive { get; private set; }

    private LastSeenModel(string userId)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        IsActive = true;
        LastSeenAt = DateTime.UtcNow;
    }

    public static IResult<LastSeenModel> Create(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Error<LastSeenModel>("UserId not set");
        }

        return Result.Success(new LastSeenModel(userId));
    }
}