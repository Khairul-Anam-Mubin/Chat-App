namespace Chat.Framework.MessageBrokers;

public class MessageEndpointProvider
{
    public static Uri GetSendEndpointUri<TMessage>(TMessage message)
    {
        return GetSendEndpointUri(message!.GetType());
    }

    public static Uri GetSendEndpointUri<TMessage>()
    {
        return GetSendEndpointUri(typeof(TMessage));
    }

    public static Uri GetSendEndpointUri(Type type)
    {
        var queueName = type.Name;

        return new Uri($"queue:{queueName}");
    }
}