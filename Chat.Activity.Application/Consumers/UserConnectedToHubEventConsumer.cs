using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class UserConnectedToHubEventConsumer : AMessageConsumer<UserConnectedToHubEvent>
{
    private readonly ICommandExecutor _commandExecutor;

    public UserConnectedToHubEventConsumer(
        ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
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

        await _commandExecutor.ExecuteAsync(updateLastSeenCommand);
    }
}