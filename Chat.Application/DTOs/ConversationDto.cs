namespace Chat.Application.DTOs;

public class ConversationDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsReceiver { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string DurationDisplayTime { get; set; } = string.Empty;
    public int Occurrence { get; set; }
    public bool IsGroupMessage { get; set; }
}