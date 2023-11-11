namespace Chat.Domain.Shared.Commands;

public class UpdateLastSeenCommand
{
    public string UserId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}