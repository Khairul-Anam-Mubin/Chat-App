namespace Chat.Framework.PubSub;

public interface IPubSub
{
    Task PublishAsync<T>(string channel, T message);
    Task PublishAsync(string channel, PubSubMessage message);
    Task SubscribeAsync<T>(string channel, Action<string, T?> handler);
}