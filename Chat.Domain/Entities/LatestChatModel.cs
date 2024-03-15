using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Domain.Entities;

public class LatestChatModel : IRepositoryItem
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string SendTo { get; private set; }
    public string Message { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }
    public int Occurrence { get; private set; }

    private LatestChatModel(string id, string userId, string sendTo, string message, DateTime sentAt, string status, bool isGroupMessage)
    {
        Id = id;
        UserId = userId;
        SendTo = sendTo;
        Message = message;
        SentAt = sentAt;
        Status = status;
        IsGroupMessage = isGroupMessage;
        Occurrence = 0;
    }

    public static LatestChatModel Create(string id, string userId, string sendTo, string message, DateTime sentAt, string status, bool isGroupMessage)
    {
        return new LatestChatModel(id, userId, sendTo, message, sentAt, status, isGroupMessage);
    }

    public bool SeenChat(string userId)
    {
        if (UserId != userId)
        {
            Occurrence = 0;
            return true;
        }
        return false;
    }

    public void Update(string userId, string sendTo, string message, string status, DateTime sentAt)
    {
        Message = message;
        SentAt = sentAt;
        Status = status;

        if (userId == UserId)
        {
            Occurrence++;
        }
        else
        {
            Occurrence = 1;
            UserId = userId;
            SendTo = sendTo;
        }
    }
}