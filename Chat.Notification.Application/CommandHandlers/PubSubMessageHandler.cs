using Chat.Framework.CQRS;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Notification.Application.Commands;

namespace Chat.Notification.Application.CommandHandlers;

public class PubSubMessageHandler : IHandler<PubSubMessage, IResponse>
{
    private readonly ICommandExecutor _commandExecutor;

    public PubSubMessageHandler(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task<IResponse> HandleAsync(PubSubMessage pubSubMessage)
    {
        if (pubSubMessage?.MessageType == MessageType.Notification)
        {
            var sendNotificationToClientCommand =
                pubSubMessage.Message.SmartCast<SendNotificationToClientCommand>();

            await _commandExecutor.ExecuteAsync(sendNotificationToClientCommand!);
        }

        return Response.Success();
    }
}