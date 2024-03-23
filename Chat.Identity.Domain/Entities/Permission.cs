using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;

namespace Chat.Identity.Domain.Entities;

public class Permission : Entity, IRepositoryItem
{
    public string Title { get; private set; }
    public List<Permission> Pemissions { get; private set; }

    private Permission(string id, string title, List<Permission> permissions) : base(id)
    {
        Title = title;
        Pemissions = permissions;
    }

    public static Permission Create(string title,  List<Permission> permissions)
    {
        return new Permission(Guid.NewGuid().ToString(), title, permissions);
    }
}
