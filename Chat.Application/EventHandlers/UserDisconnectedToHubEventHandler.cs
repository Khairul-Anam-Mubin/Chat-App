using Chat.Domain.Events;
using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.EventHandlers;

[ServiceRegister(typeof(IRequestHandler<UserDisconnectedToHubEvent>), ServiceLifetime.Singleton)]
public class UserDisconnectedToHubEventHandler : IRequestHandler<UserDisconnectedToHubEvent>
{
    private readonly ICommandQueryProxy _commandQueryProxy;

    public UserDisconnectedToHubEventHandler(ICommandQueryProxy commandQueryProxy)
    {
        _commandQueryProxy = commandQueryProxy;
    }

    public async Task HandleAsync(UserDisconnectedToHubEvent request)
    {
        await TrackLastSeenActivityAsync(request.UserId, false);
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
        await _commandQueryProxy.GetCommandResponseAsync(updateLastSeenCommand);
    }
}