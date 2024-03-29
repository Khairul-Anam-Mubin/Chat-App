﻿using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Events;
using Chat.Framework.CQRS;
using Chat.Framework.EDD;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Activity.Application.Consumers;

public class UserConnectedToHubEventConsumer : AEventConsumer<UserConnectedToHubEvent>
{
    private readonly ICommandExecutor _commandExecutor;

    public UserConnectedToHubEventConsumer(
        ICommandExecutor commandExecutor, IScopeIdentity scopeIdentity) 
        : base(scopeIdentity)
    {
        _commandExecutor = commandExecutor;
    }

    protected override async Task OnConsumeAsync(UserConnectedToHubEvent @event, IMessageContext<UserConnectedToHubEvent>? context = null)
    {
        await TrackPresenceAsync(@event, true);
    }

    private async Task TrackPresenceAsync(UserConnectedToHubEvent userConnectedToHubEvent, bool isActive)
    {
        var trackPresentCommand = new TrackPresenceCommand
        {
            UserId = userConnectedToHubEvent.UserId,
            IsActive = isActive
        };

        await _commandExecutor.ExecuteAsync(trackPresentCommand);
    }
}