using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Entities;

public class Role : Entity, IRepositoryItem
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public List<string> PermissionIds { get; private set; }

    private Role(string id, string title, string description) : base(id)
    {
        Title = title;
        Description = description;
        PermissionIds = new();
    }

    public static Role Create(string title, string description)
    {
        return new Role(Guid.NewGuid().ToString(), description, title);
    }

    public void AddPermission(Permission permission)
    {
        PermissionIds.Add(permission.Id);
    }

    public void AddPermissions(List<Permission> permissions)
    {
        PermissionIds.AddRange(permissions.Select(permission => permission.Id).ToList());
    }
}
