namespace Chat.Framework.MessageBrokers;

public interface IMessageRequestClient
{
    Task<TResponse> GetResponseAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class;
}