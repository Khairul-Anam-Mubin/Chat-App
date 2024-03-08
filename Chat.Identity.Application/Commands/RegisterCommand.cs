using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class RegisterCommand : ICommand
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDay { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}