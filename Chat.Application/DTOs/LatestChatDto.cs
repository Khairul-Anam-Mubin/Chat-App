namespace Chat.Application.DTOs;

public class LatestChatDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsReceiver { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string DurationDisplayTime { get; set; } = string.Empty;
    public int Occurrence { get; set; }
}