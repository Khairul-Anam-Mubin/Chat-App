using Chat.Framework.Database.Interfaces;

namespace Chat.Domain.Models;

public class ChatModel : IEntity
{
    public string Id {get; set;} = string.Empty;
    public string UserId {get; set;} = string.Empty;
    public string SendTo {get; set;} = string.Empty;
    public string Message {get; set;} = string.Empty;
    public DateTime SentAt {get; set;}
    public string Status {get; set;} = string.Empty;
}