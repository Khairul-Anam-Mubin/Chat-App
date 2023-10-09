using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class RegisterCommand : ACommand
{
    public UserModel UserModel { get; set; }
}