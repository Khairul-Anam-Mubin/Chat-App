using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Activity.Domain.Models;

public class LastSeenModel : IEntity
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime LastSeenAt { get; set; }
    public bool IsActive { get; set; }

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