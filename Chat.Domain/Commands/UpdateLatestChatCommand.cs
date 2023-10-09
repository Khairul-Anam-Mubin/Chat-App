using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class UpdateLatestChatCommand : ACommand
{
    public LatestChatModel? LatestChatModel {get; set;}
}