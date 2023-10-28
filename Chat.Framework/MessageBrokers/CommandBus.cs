using MassTransit;

namespace Chat.Framework.MessageBrokers;

public class CommandBus : ICommandBus
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public CommandBus(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }
    
    public async Task SendAsync<TCommand>(TCommand command) where TCommand : class
    {
        var uri = MessageEndpointProvider.GetSendEndpointUri(command);
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(uri);
        await endpoint.Send(command);
    }
}