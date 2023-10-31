using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Proxy;

namespace Chat.Activity.Application.EventHandlers;

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
            ApiUrl = "https://localhost:50502/api/Activity/track",
            IsActive = isActive,
            FireAndForget = true
        };
        await _commandService.GetResponseAsync(updateLastSeenCommand);
    }
}