using Chat.Notification.Application.Commands;
using KCluster.Framework.CQRS;
using KCluster.Framework.Extensions;
using KCluster.Framework.Mediators;
using KCluster.Framework.PubSub;

namespace Chat.Notification.Application.CommandHandlers;

public class PubSubMessageHandler : IHandler<PubSubMessage>
{
    private readonly ICommandExecutor _commandExecutor;

    public PubSubMessageHandler(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task HandleAsync(PubSubMessage pubSubMessage)
    {
        switch (pubSubMessage.MessageType)
        {
            case MessageType.Notification:
                var sendNotificationToClientCommand =
                    pubSubMessage.Message.SmartCast<SendNotificationToClientCommand>();
                await _commandExecutor.ExecuteAsync(sendNotificationToClientCommand!);
                break;
            default:
                Console.WriteLine("MessageType not specified");
                break;
        }
    }
}