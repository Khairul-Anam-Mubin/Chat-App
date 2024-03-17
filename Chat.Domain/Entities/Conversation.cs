using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;

namespace Chat.Domain.Entities;

public class Conversation : AggregateRoot, IRepositoryItem
{
    public string UserId { get; private set; }
    public string SendTo { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public string Status { get; private set; }
    public bool IsGroupMessage { get; private set; }
    public int Occurrence { get; private set; }

    private Conversation(string id, string userId, string sendTo, string messageContent, DateTime sentAt, string status, bool isGroupMessage)
        : base(id)
    {
        UserId = userId;
        SendTo = sendTo;
        Content = messageContent;
        SentAt = sentAt;
        Status = status;
        IsGroupMessage = isGroupMessage;
        Occurrence = 0;
    }

    public static Conversation Create(string id, string userId, string sendTo, string messageContent, DateTime sentAt, string status, bool isGroupMessage)
    {
        return new Conversation(id, userId, sendTo, messageContent, sentAt, status, isGroupMessage);
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

    public void Update(string userId, string sendTo, string messageContent, string status, DateTime sentAt)
    {
        Content = messageContent;
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