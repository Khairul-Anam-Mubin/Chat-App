using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Proxy;

namespace Chat.Activity.Application.EventHandlers;

public class UserConnectedToHubEventConsumer : AMessageConsumer<UserConnectedToHubEvent>
{
    private readonly ICommandService _commandService;
    private readonly ICommandBus _commandBus;

    public UserConnectedToHubEventConsumer(
        ICommandService commandService,
        ICommandBus commandBus)
    {
        _commandService = commandService;
        _commandBus = commandBus;
    }

    public override async Task Consume(IMessageContext<UserConnectedToHubEvent> context)
    {
        var connectedEvent = context.Message;
        Console.WriteLine(context.Message.Serialize());
        await TrackLastSeenActivityAsync(connectedEvent.UserId, true, context);
    }

    private async Task TrackLastSeenActivityAsync(string userId, bool isActive, IMessageContext<UserConnectedToHubEvent> context = null)
    {
        var updateLastSeenCommand = new UpdateLastSeenCommand
        {
            UserId = userId,
            ApiUrl = "https://localhost:50502/api/Activity/track",
            IsActive = isActive,
            FireAndForget = true
        };

        await _commandService.GetResponseAsync<UpdateLastSeenCommand, CommandResponse>(updateLastSeenCommand);
    }
}