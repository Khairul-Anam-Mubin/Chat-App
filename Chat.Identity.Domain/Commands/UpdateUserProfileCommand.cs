using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class UpdateUserProfileCommand
{
    public UserModel UserModel { get; set; }
}