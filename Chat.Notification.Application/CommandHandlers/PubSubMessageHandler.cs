using Chat.Framework.CQRS;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.Results;
using Chat.Notification.Application.Commands;

namespace Chat.Notification.Application.CommandHandlers;

public class PubSubMessageHandler : IHandler<PubSubMessage, IResult>
{
    private readonly ICommandExecutor _commandExecutor;

    public PubSubMessageHandler(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task<IResult> HandleAsync(PubSubMessage pubSubMessage)
    {
        if (pubSubMessage?.MessageType == MessageType.Notification)
        {
            var sendNotificationToClientCommand =
                pubSubMessage.Message.SmartCast<SendNotificationToClientCommand>();

            await _commandExecutor.ExecuteAsync(sendNotificationToClientCommand!);
        }

        return Result.Success();
    }
}