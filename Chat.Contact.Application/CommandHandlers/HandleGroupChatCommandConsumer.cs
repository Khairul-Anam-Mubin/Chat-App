using Chat.Contact.Domain.Interfaces;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class HandleGroupChatCommandConsumer : ACommandConsumer<HandleGroupChatCommand>
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly ICommandBus _commandBus;

    public HandleGroupChatCommandConsumer(IGroupMemberRepository groupMemberRepository, ICommandBus commandBus)
    {
        _groupMemberRepository = groupMemberRepository;
        _commandBus = commandBus;
    }

    protected override async Task<IResult> OnConsumeAsync(HandleGroupChatCommand command, IMessageContext<HandleGroupChatCommand>? context = null)
    {
        var groupMembers = 
            await _groupMemberRepository.GetAllGroupMembers(command.GroupId);

        var groupMemberIds = groupMembers.Select(x => x.Id).ToList();

        var notification = new NotificationData("GroupChat", command.ChatId , "ChatId", command.SenderId);

        var sendNotificationCommand = new SendNotificationCommand(notification, groupMemberIds);

        await _commandBus.SendAsync(sendNotificationCommand);

        return Result.Success();
    }
}
