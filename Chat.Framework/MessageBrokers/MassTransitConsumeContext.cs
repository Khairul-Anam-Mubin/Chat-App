using MassTransit;

namespace Chat.Framework.MessageBrokers;

public class MassTransitConsumeContext<TMessage> : IMessageContext<TMessage> where TMessage : class
{
    private readonly ConsumeContext<TMessage> _consumeContext;

    public MassTransitConsumeContext(ConsumeContext<TMessage> consumeContext)
    {
        _consumeContext = consumeContext;
        Message = _consumeContext.Message;
    }

    public TMessage Message { get; }

    public async Task RespondAsync<T>(T message) where T : class
    {
        await _consumeContext.RespondAsync(message);
    }

    public async Task PublishAsync<TEvent>(TEvent message) where TEvent : class
    {
        await _consumeContext.Publish(message);
    }

    public async Task SendAsync<TCommand>(TCommand command) where TCommand : class
    {
        var uri = MessageEndpointProvider.GetSendEndpointUri(command);
        var sendEndpoint = await _consumeContext.GetSendEndpoint(uri);
        await sendEndpoint.Send(command);
    }
}