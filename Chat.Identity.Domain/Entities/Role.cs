using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Identity.Domain.Entities;

public class Role : Entity, IRepositoryItem
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public List<Permission> Permissions { get; private set; }

    private Role(string id, string title, string description, List<Permission> permissions) : base(id)
    {
        Title = title;
        Description = description;
        Permissions = permissions;
    }

    public static Role Create(string title, string description, List<Permission> permissions)
    {
        return new Role(Guid.NewGuid().ToString(), description, title, permissions);
    }
}
