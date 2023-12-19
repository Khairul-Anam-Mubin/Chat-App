using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.PubSub;
using Chat.Notification.Domain.Interfaces;

namespace Chat.Notification.Infrastructure.PubSub;

public sealed class PubSubMessageSubscriber : IInitialService
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IMediator _mediator;
    private readonly IPubSub _pubSub;

    public PubSubMessageSubscriber(
        IHubConnectionService hubConnectionService, 
        IMediator mediator, 
        IPubSub pubSub)
    {
        _hubConnectionService = hubConnectionService;
        _mediator = mediator;
        _pubSub = pubSub;
    }

    public async Task InitializeAsync()
    {
        var channel = _hubConnectionService.GetCurrentHubInstanceId();

        Console.WriteLine($"Subscribing to redis channel : {channel}\n");

        await _pubSub.SubscribeAsync<PubSubMessage>(channel, (redisChannel, message) =>
        {
            if (message is null)
            {
                Console.WriteLine("Message is null here");
                return;
            }

            Console.WriteLine($"PubSubMessage.Id : {message?.Id}, PubSubMessageType: {message?.MessageType.ToString()} , message : {message}\n");

            Task.Factory.StartNew(() => _mediator.SendAsync(message));
        });
    }
}