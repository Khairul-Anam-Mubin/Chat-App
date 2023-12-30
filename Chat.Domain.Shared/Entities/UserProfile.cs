using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Entities;

public class UserProfile
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    [Required]
    public DateTime? BirthDay { get; set; }

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string About { get; set; } = string.Empty;

    public string ProfilePictureId { get; set; } = string.Empty;

    public int PublicKey { get; set; }
}