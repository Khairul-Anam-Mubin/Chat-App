namespace Chat.Framework.MessageBrokers;

public class MessageEndpointProvider
{
    public static Uri GetSendEndpointUri<TMessage>(TMessage message)
    {
        var queueName = typeof(TMessage).Name;

        return new Uri($"queue:{queueName}");
    }
}