namespace Chat.Domain.Commands;

public class UpdateChatsStatusCommand
{
    public string UserId {get; set;} = string.Empty;
    public string OpenedChatUserId {get; set;} = string.Empty;
}