using Chat.Framework.CQRS;

namespace Chat.Application.Commands;

public class UpdateChatsStatusCommand : ICommand
{
    public string UserId { get; set; } = string.Empty;
    public string OpenedChatUserId { get; set; } = string.Empty;
}