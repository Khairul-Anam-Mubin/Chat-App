using Chat.Contacts.Domain.Repositories;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.MessageBrokers;
using Peacious.Framework.Results;

namespace Chat.Contacts.Application.CommandHandlers;

public class HandleGroupMessageCommandConsumer : ACommandConsumer<HandleGroupMessageCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly ICommandService _commandService;

    public HandleGroupMessageCommandConsumer(
        IGroupRepository groupRepository,
        IScopeIdentity scopeIdentity,
        ICommandService commandService,
        IGroupMemberRepository groupMemberRepository)
        : base(scopeIdentity)
    {
        _groupRepository = groupRepository;
        _commandService = commandService;
        _groupMemberRepository = groupMemberRepository;
    }

    protected override async Task<IResult> OnConsumeAsync(HandleGroupMessageCommand command, IMessageContext<HandleGroupMessageCommand>? context = null)
    {
        var groupMembers =
            await _groupMemberRepository.GetAllGroupMembersAsync(command.GroupId);

        var groupMemberIds = groupMembers.Select(x => x.MemberId).ToList();

        var notification =
            new NotificationData(GetGroupChatTopic(command.GroupId), command.ChatId, "ChatId", command.SenderId);

        var sendNotificationCommand = new SendNotificationCommand(notification, groupMemberIds);

        await _commandService.SendAsync(sendNotificationCommand);

        return Result.Success();
    }

    private string GetGroupChatTopic(string groupId)
    {
        return "GroupChat-" + groupId;
    }
}
