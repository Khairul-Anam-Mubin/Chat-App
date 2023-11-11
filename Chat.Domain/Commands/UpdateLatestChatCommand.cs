using Chat.Domain.Models;

namespace Chat.Domain.Commands;

public class UpdateLatestChatCommand
{
    public LatestChatModel? LatestChatModel {get; set;}
}