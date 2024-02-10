using Chat.Framework.Identity;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.PubSub;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Infrastructure.PubSub;

public sealed class PubSubMessageSubscriber : IInitialService
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IPubSub _pubSub;

    public PubSubMessageSubscriber(
        IHubConnectionService hubConnectionService, 
        IServiceScopeFactory serviceScopeFactory,
        IPubSub pubSub)
    {
        _hubConnectionService = hubConnectionService;
        _serviceScopeFactory = serviceScopeFactory;
        _pubSub = pubSub;
    }

    public async Task InitializeAsync()
    {
        var channel = _hubConnectionService.GetCurrentHubId();

        Console.WriteLine($"Subscribing to redis channel : {channel}\n");

        await _pubSub.SubscribeAsync<PubSubMessage>(channel, async (redisChannel, message) =>
        {
            if (message is null)
            {
                Console.WriteLine("Message is null here");
                return;
            }

            Console.WriteLine($"PubSubMessage.Id : {message?.Id}, PubSubMessageType: {message?.MessageType.ToString()} , message : {message}\n");

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var scopeIdentity = scope.ServiceProvider.GetRequiredService<IScopeIdentity>();
            var accessToken = message!.Token;
            // Todo: validate token as its the entry point
            scopeIdentity.SwitchIdentity(accessToken);
            await mediator.SendAsync(message);
        });
    }
}