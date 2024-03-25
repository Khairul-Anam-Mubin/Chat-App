using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;

namespace Chat.Identity.Domain.Entities;

public class PermissionAccess : Entity, IRepositoryItem
{
    public string PermissionId { get; private set; }
    public string UserId { get; private set; }

    private PermissionAccess(string id, string permissionId, string userId) : base(id)
    {
        PermissionId = permissionId;
        UserId = userId;
    }

    public static PermissionAccess Create(string permissionId, string userId)
    {
        return new PermissionAccess(Guid.NewGuid().ToString(), permissionId, userId);
    }
}
