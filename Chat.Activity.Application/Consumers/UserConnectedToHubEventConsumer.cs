using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Models;

namespace Chat.Activity.Application.Consumers;

public class UserConnectedToHubEventConsumer : AMessageConsumer<UserConnectedToHubEvent>
{
    private readonly ICommandService _commandService;

    public UserConnectedToHubEventConsumer(
        ICommandService commandService)
    {
        _commandService = commandService;
    }

    public override async Task Consume(IMessageContext<UserConnectedToHubEvent> context)
    {
        await TrackLastSeenActivityAsync(context.Message.UserId, true);
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