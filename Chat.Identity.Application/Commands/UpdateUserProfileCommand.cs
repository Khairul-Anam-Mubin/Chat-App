using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Commands;

public class UpdateUserProfileCommand : ICommand
{
    public UserModel UserModel { get; set; }
}