using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Commands;

public class UpdateUserProfileCommand
{
    public UserModel UserModel { get; set; }
}