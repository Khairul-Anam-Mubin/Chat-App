using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Models;
using Chat.Notification.Domain.Commands;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application.Consumers;

[ServiceRegister(typeof(IRequestHandler<SendNotificationCommand, Response>), ServiceLifetime.Transient)]
public class SendNotificationCommandConsumer :
    ACommandConsumer<SendNotificationCommand, Response>
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly ICommandService _commandService;

    public SendNotificationCommandConsumer(
        IHubConnectionService hubConnectionService,
        ICommandService commandService)
    {
        _hubConnectionService = hubConnectionService;
        _commandService = commandService;
    }

    protected override async Task<Response> OnConsumeAsync(SendNotificationCommand command, IMessageContext<SendNotificationCommand>? context = null)
    {
        var receiver = command.Receiver!;

        var hubIdUserIdsMapper = new Dictionary<string, List<string>>();

        foreach (var receiverId in receiver.ReceiverIds)
        {
            var hubInstanceId =
                await _hubConnectionService.GetUserConnectedHubInstanceIdAsync(receiverId) ??
                string.Empty;

            if (string.IsNullOrEmpty(hubInstanceId))
            {
                Console.WriteLine($"Receiver : {receiver} is not connected with any hub");
                continue;
            }

            if (hubIdUserIdsMapper.TryGetValue(hubInstanceId, out var value))
            {
                value.Add(receiverId);
            }
            else
            {
                hubIdUserIdsMapper.Add(hubInstanceId, new List<string> { receiverId });
            }
        }

        foreach (var keyPair in hubIdUserIdsMapper)
        {
            if (_hubConnectionService.GetCurrentHubInstanceId() == keyPair.Key)
            {
                await SendNotificationToClientAsync(keyPair.Value, command.Notification!);
            }
            else
            {
                await PublishNotificationToConnectedHubAsync(keyPair.Key, keyPair.Value, command.Notification!);
            }
        }

        return Response.Create();
    }

    private async Task PublishNotificationToConnectedHubAsync(string hubId, List<string> receiverIds, Chat.Domain.Shared.Entities.Notification notification)
    {
        var publishNotificationToConnectedHubCommand = new PublishNotificationToConnectedHubCommand
        {
            HubInstanceId = hubId,
            Notification = notification,
            ReceiverIds = receiverIds
        };

        await _commandService.GetResponseAsync(publishNotificationToConnectedHubCommand);
    }

    private async Task SendNotificationToClientAsync(List<string> receiverIds, Chat.Domain.Shared.Entities.Notification notification)
    {
        var sendNotificationToClientCommand = new SendNotificationToClientCommand
        {
            Notification = notification,
            ReceiverIds = receiverIds
        };

        await _commandService.GetResponseAsync(sendNotificationToClientCommand);
    }
}