using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;

namespace Chat.Identity.Domain.Entities;

public class RoleAccess : Entity, IRepositoryItem
{
    public string RoleId { get; private set; }
    public string UserId { get; private set; }

    private RoleAccess(string id, string roleId, string userId) : base(id)
    {
        RoleId = roleId;
        UserId = userId;
    }

    public static RoleAccess Create(string roleId, string userId)
    {
        return new RoleAccess(Guid.NewGuid().ToString(), roleId, userId);
    }
}
