using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;

namespace Chat.Identity.Domain.Entities;

public class PermissionAccess : Entity, IRepositoryItem
{
    public string PermissionId { get; private set; }
    public string UserId { get; private set; }
    public DateTime ExpirationTime { get; private set; }                                                                                                            
    public bool UnlimitedLifeTime { get; private set; }

    private PermissionAccess(string id, string permissionId, string userId, DateTime expirationTime, bool unlimitedLifeTime) : base(id)
    {
        PermissionId = permissionId;
        UserId = userId;
        ExpirationTime = expirationTime;
        UnlimitedLifeTime = unlimitedLifeTime;
    }

    public static PermissionAccess Create(string permissionId, string userId, DateTime expirationTime, bool unlimitedLifeTime)
    {
        return new PermissionAccess(Guid.NewGuid().ToString(), permissionId, userId, expirationTime, unlimitedLifeTime);
    }

    public bool IsExpired()
    {
        return ExpirationTime < DateTime.UtcNow;
    }
}
