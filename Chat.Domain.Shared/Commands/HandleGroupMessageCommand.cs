using Chat.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Commands;

public class HandleGroupMessageCommand : ICommand, IInternalMessage
{
    [Required]
    public string GroupId { get; set; }
    
    [Required]
    public string SenderId { get; set; }
    
    [Required]
    public string ChatId { get; set; }
    public string? Token { get; set; }

    public HandleGroupMessageCommand(string groupId, string senderId, string chatId)
    {
        GroupId = groupId;
        SenderId = senderId;
        ChatId = chatId;
    }
}
