using Chat.Framework.CQRS;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Notification.Application.Commands;

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