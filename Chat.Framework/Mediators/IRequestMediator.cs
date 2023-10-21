namespace Chat.Framework.Mediators;

public interface IRequestMediator
{
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request);
    Task SendAsync<TRequest>(TRequest request);
}