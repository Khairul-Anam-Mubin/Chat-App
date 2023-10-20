namespace Chat.Framework.Proxy;

public interface IProxy
{
    Task SendAsync<TRequest>(TRequest request);
}