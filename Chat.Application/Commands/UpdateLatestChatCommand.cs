using Chat.Domain.Models;

namespace Chat.Application.Commands;

public class UpdateLatestChatCommand
{
    public LatestChatModel? LatestChatModel { get; set; }
}