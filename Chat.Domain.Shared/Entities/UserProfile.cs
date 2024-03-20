using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Entities;

public class UserProfile
{
    public string Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string UserName { get; set; }

    [Required]
    public DateTime? BirthDay { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(1000)]
    public string? About { get; set; }

    public string? ProfilePictureId { get; set; }
}