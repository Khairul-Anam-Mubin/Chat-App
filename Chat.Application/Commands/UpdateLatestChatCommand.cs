using Chat.Domain.Entities;
using Chat.Framework.CQRS;

namespace Chat.Application.Commands;

public class UpdateLatestChatCommand : ICommand
{
    public LatestChatModel? LatestChatModel { get; set; }
}