using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Models;

namespace Chat.Activity.Application.Consumers;

public class UserDisconnectedToHubEventConsumer : AMessageConsumer<UserDisconnectedToHubEvent>
{
    private readonly ICommandService _commandService;

    public UserDisconnectedToHubEventConsumer(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public override async Task Consume(IMessageContext<UserDisconnectedToHubEvent> context)
    {
        await TrackLastSeenActivityAsync(context.Message.UserId, false);
    }

    private async Task TrackLastSeenActivityAsync(string userId, bool isActive)
    {
        var updateLastSeenCommand = new UpdateLastSeenCommand
        {
            UserId = userId,
            IsActive = isActive
        };
        await _commandService.GetResponseAsync<UpdateLastSeenCommand, Response>(updateLastSeenCommand);
    }
}