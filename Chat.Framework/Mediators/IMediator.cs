namespace Chat.Framework.Mediators;

public interface IMediator
{
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request);
    Task SendAsync<TRequest>(TRequest request);
}