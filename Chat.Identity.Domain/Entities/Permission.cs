using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Entities;

public class Permission : Entity, IRepositoryItem
{
    public string Title { get; private set; }
    public List<string> PermissionIds { get; private set; }

    private Permission(string id, string title) : base(id)
    {
        Title = title;
        PermissionIds = new();
    }

    public static Permission Create(string title)
    {
        return new Permission(Guid.NewGuid().ToString(), title);
    }

    public void AddChildPermission(Permission permission)
    {
        PermissionIds.Add(permission.Id);
    }

    public void AddChildPermissions(List<Permission> permissions)
    {
        PermissionIds.AddRange(permissions.Select(permission => permission.Id).ToList());
    }
}
