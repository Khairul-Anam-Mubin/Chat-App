using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.PubSub;

[ServiceRegister(typeof(IRequestHandler<PubSubMessage>), ServiceLifetime.Transient)]
public class PubSubMessageHandler : IRequestHandler<PubSubMessage>
{
    private readonly ICommandService _commandService;
    
    public PubSubMessageHandler(ICommandService commandService)
    {
        _commandService = commandService;
    }
    
    public async Task HandleAsync(PubSubMessage pubSubMessage)
    {
        if (pubSubMessage?.MessageType == MessageType.Notification)
        {
            var sendNotificationToClientCommand =
                pubSubMessage.Message.SmartCast<SendNotificationToClientCommand>();

            await _commandService.GetResponseAsync(sendNotificationToClientCommand!);
        }
    }
}