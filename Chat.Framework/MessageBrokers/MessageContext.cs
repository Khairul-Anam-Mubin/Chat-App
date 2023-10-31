using MassTransit;

namespace Chat.Framework.MessageBrokers;

public class MessageContext<TMessage> : IMessageContext<TMessage> where TMessage : class
{
    private readonly ConsumeContext<TMessage> _consumeContext;

    public MessageContext(ConsumeContext<TMessage> consumeContext)
    {
        _consumeContext = consumeContext;
        Message = _consumeContext.Message;
    }

    public TMessage Message { get; }

    public async Task RespondAsync<TResponse>(TResponse response) where TResponse : class
    {
        await _consumeContext.RespondAsync(response);
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