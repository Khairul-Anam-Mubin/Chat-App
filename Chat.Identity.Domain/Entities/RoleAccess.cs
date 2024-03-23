using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;

namespace Chat.Identity.Domain.Entities;

public class RoleAccess : Entity, IRepositoryItem
{
    public string RoleId { get; private set; }
    public string UserId { get; private set; }
    public DateTime ExpirationTime { get; private set; }
    public bool UnlimitedLifeTime { get; private set; }

    private RoleAccess(string id, string roleId, string userId, DateTime expirationTime, bool unlimitedLifeTime) : base(id)
    {
        RoleId = roleId;
        UserId = userId;
        ExpirationTime = expirationTime;
        UnlimitedLifeTime = unlimitedLifeTime;
    }

    public static RoleAccess Create(string roleId, string userId, DateTime expirationTime, bool unlimitedLifeTime)
    {
        return new RoleAccess(Guid.NewGuid().ToString(), roleId, userId, expirationTime, unlimitedLifeTime);
    }

    public bool IsExpired()
    {
        return ExpirationTime < DateTime.UtcNow;
    }
}
