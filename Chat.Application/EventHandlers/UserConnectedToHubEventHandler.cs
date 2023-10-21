using Chat.Domain.Events;
using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.EventHandlers;

[ServiceRegister(typeof(IRequestHandler<UserConnectedToHubEvent>), ServiceLifetime.Singleton)]
public class UserConnectedToHubEventHandler : IRequestHandler<UserConnectedToHubEvent>
{
    private readonly ICommandQueryProxy _commandQueryProxy;
    
    public UserConnectedToHubEventHandler(ICommandQueryProxy commandQueryProxy)
    {
        _commandQueryProxy = commandQueryProxy;
    }
    
    public async Task HandleAsync(UserConnectedToHubEvent request)
    {
        await TrackLastSeenActivityAsync(request.UserId, true);
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
