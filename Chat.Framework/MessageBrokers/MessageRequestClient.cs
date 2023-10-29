using MassTransit;

namespace Chat.Framework.MessageBrokers;

public class MessageRequestClient : IMessageRequestClient
{
    private readonly IScopedClientFactory _scopedClientFactory;

    public MessageRequestClient(IScopedClientFactory scopedClientFactory)
    {
        _scopedClientFactory = scopedClientFactory;
    }

    public async Task<TResponse> GetResponseAsync<TRequest, TResponse>(TRequest request) 
        where TRequest : class 
        where TResponse : class
    {
        var uri = MessageEndpointProvider.GetSendEndpointUri(request);

        var client = _scopedClientFactory.CreateRequestClient<TRequest>(uri);
        
        var response = await client.GetResponse<TResponse>(request);
        
        return response.Message;
    }
}