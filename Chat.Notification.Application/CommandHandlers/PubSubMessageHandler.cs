using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Notification.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<PubSubMessage, IResponse>), ServiceLifetime.Transient)]
public class PubSubMessageHandler : IRequestHandler<PubSubMessage, IResponse>
{
    private readonly ICommandService _commandService;

    public PubSubMessageHandler(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task<IResponse> HandleAsync(PubSubMessage pubSubMessage)
    {
        if (pubSubMessage?.MessageType == MessageType.Notification)
        {
            var sendNotificationToClientCommand =
                pubSubMessage.Message.SmartCast<SendNotificationToClientCommand>();

            await _commandService.GetResponseAsync(sendNotificationToClientCommand!);
        }

        return Response.Success();
    }
}