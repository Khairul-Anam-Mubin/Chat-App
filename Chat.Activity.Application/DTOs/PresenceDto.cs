namespace Chat.Activity.Application.DTOs;

public class PresenceDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime LastSeenAt { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; } = string.Empty;
}