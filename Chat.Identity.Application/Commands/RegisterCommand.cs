using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Commands;

public class RegisterCommand : ICommand
{
    [Required]
    public UserModel UserModel { get; set; }
}