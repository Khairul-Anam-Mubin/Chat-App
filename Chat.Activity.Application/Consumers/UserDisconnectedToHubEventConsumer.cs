using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.EDD;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class UserDisconnectedToHubEventConsumer : AEventConsumer<UserDisconnectedToHubEvent>
{
    private readonly ICommandExecutor _commandExecutor;

    public UserDisconnectedToHubEventConsumer(ICommandExecutor commandExecutor, IScopeIdentity scopeIdentity)
        : base(scopeIdentity) 
    {
        _commandExecutor = commandExecutor;
    }

    protected override async Task OnConsumeAsync(UserDisconnectedToHubEvent @event, IMessageContext<UserDisconnectedToHubEvent>? context = null)
    {
        await TrackLastSeenActivityAsync(@event, false);
    }

    private async Task TrackLastSeenActivityAsync(UserDisconnectedToHubEvent disconnectedToHubEvent, bool isActive)
    {
        var updateLastSeenCommand = new UpdateLastSeenCommand
        {
            UserId = disconnectedToHubEvent.UserId,
            IsActive = isActive
        };
        await _commandExecutor.ExecuteAsync(updateLastSeenCommand);
    }
}