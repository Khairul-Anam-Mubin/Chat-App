using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class UpdateUserProfileCommand : ACommand
{
    public UserModel UserModel { get; set; }
}