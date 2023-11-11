using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Commands;

public class RegisterCommand
{
    public UserModel UserModel { get; set; }
}