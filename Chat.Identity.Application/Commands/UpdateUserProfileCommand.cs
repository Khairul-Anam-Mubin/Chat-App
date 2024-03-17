using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Application.Commands;

public class UpdateUserProfileCommand : ICommand
{
    [Required]
    public User UserModel { get; set; }
}