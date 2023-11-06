using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Notification.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<PubSubMessage, Response>), ServiceLifetime.Transient)]
public class PubSubMessageHandler : IRequestHandler<PubSubMessage, Response>
{
    private readonly ICommandService _commandService;

    public PubSubMessageHandler(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task<Response> HandleAsync(PubSubMessage pubSubMessage)
    {
        if (pubSubMessage?.MessageType == MessageType.Notification)
        {
            var sendNotificationToClientCommand =
                pubSubMessage.Message.SmartCast<SendNotificationToClientCommand>();

            await _commandService.GetResponseAsync(sendNotificationToClientCommand!);
        }

        return Response.Create();
    }
}