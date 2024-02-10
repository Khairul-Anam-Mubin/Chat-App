using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class UserDisconnectedToHubEventConsumer : AMessageConsumer<UserDisconnectedToHubEvent>
{
    private readonly ICommandExecutor _commandExecutor;

    public UserDisconnectedToHubEventConsumer(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public override async Task Consume(IMessageContext<UserDisconnectedToHubEvent> context)
    {
        await TrackLastSeenActivityAsync(context.Message, false);
    }

    private async Task TrackLastSeenActivityAsync(UserDisconnectedToHubEvent disconnectedToHubEvent, bool isActive)
    {
        var updateLastSeenCommand = new UpdateLastSeenCommand
        {
            UserId = disconnectedToHubEvent.UserId,
            IsActive = isActive,
            Token = disconnectedToHubEvent.Token,
        };
        await _commandExecutor.ExecuteAsync(updateLastSeenCommand);
    }
}