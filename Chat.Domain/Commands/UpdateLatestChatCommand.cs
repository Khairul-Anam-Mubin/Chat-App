using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class UpdateLatestChatCommand
{
    public LatestChatModel? LatestChatModel {get; set;}
}