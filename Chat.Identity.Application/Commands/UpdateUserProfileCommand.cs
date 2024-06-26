using Chat.Domain.Shared.Entities;
using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Application.Commands;

public class UpdateUserProfileCommand : ICommand<UserProfile>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDay { get; set; }

    [MaxLength(1000)]
    public string? About { get; set; }

    public string? ProfilePictureId { get; set; }
}