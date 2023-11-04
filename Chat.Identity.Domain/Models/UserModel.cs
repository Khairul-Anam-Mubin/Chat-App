using Chat.Domain.Shared.Entities;
using Chat.Framework.Database.Interfaces;

namespace Chat.Identity.Domain.Models;

public class UserModel : UserProfile, IEntity
{
    public string Password { get; set; } = string.Empty;
}