using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Entities;

public class UserAccess : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public List<string> RoleIds { get; private set; }
    public List<string> PermissionIds { get; private set; }

    private UserAccess(string id, string userId) : base(id)
    {
        UserId = userId;
        RoleIds = new List<string>();
        PermissionIds = new List<string>();
    }

    public static UserAccess Create(string id, string userId)
    {
        return new UserAccess(id, userId);
    }

    public void AddPermission(Permission permission)
    {
        PermissionIds.Add(permission.Id);
    }

    public void Role(Role role)
    {
        RoleIds.Add(role.Id);
    }
}
