using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Entities;

public class Permission : Entity, IRepositoryItem
{
    public string Title { get; private set; }
    public List<Permission> Permissions { get; private set; }

    private Permission(string id, string title, List<Permission> permissions) : base(id)
    {
        Title = title;
        Permissions = permissions;
    }

    public static Permission Create(string title,  List<Permission> permissions)
    {
        return new Permission(Guid.NewGuid().ToString(), title, permissions);
    }
}
