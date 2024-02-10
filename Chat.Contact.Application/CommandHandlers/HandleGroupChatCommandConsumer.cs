using Chat.Contact.Domain.Interfaces;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class HandleGroupChatCommandConsumer : ACommandConsumer<HandleGroupChatCommand>
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly ICommandService _commandService;

    public HandleGroupChatCommandConsumer(
        IGroupMemberRepository groupMemberRepository, 
        IScopeIdentity scopeIdentity,
        ICommandService commandService)
        : base(scopeIdentity)
    {
        _groupMemberRepository = groupMemberRepository;
        _commandService = commandService;
    }

    protected override async Task<IResult> OnConsumeAsync(HandleGroupChatCommand command, IMessageContext<HandleGroupChatCommand>? context = null)
    {
        var groupMembers = 
            await _groupMemberRepository.GetAllGroupMembers(command.GroupId);

        var groupMemberIds = groupMembers.Select(x => x.MemberId).ToList();

        var notification = 
            new NotificationData(GetGroupChatTopic(command.GroupId), command.ChatId , "ChatId", command.SenderId);

        var sendNotificationCommand = new SendNotificationCommand(notification, groupMemberIds);
        
        await _commandService.SendAsync(sendNotificationCommand);

        return Result.Success();
    }

    private string GetGroupChatTopic(string groupId)
    {
        return "GroupChat-" + groupId;
    }
}
