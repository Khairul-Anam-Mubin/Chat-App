using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class UpdateChatsStatusCommand : ACommand
{
    public string UserId {get; set;} = string.Empty;
    public string OpenedChatUserId {get; set;} = string.Empty;
}