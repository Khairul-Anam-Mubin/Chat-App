using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class RegisterCommand
{
    public UserModel UserModel { get; set; }
}