using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Activity.Domain.Models;

public class LastSeenModel : IEntity
{
    public string Id { get; set; }
    public string UserId { get; }
    public DateTime LastSeenAt { get; }
    public bool IsActive { get; }

    private LastSeenModel(string userId)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        IsActive = true;
        LastSeenAt = DateTime.UtcNow;
    }

    public static LastSeenModel Create(string userId)
    {
        return new LastSeenModel(userId);
    }
}