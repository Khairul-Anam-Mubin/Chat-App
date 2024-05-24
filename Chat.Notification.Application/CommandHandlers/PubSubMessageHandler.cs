using Chat.Notification.Application.Commands;
using Peacious.Framework.CQRS;
using Peacious.Framework.Extensions;
using Peacious.Framework.Mediators;
using Peacious.Framework.PubSub;

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