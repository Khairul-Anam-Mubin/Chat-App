using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Framework.RequestResponse;
using Chat.Notification.Application.Commands;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application.Consumers;

[ServiceRegister(typeof(IHandler<SendNotificationCommand, IResponse>), ServiceLifetime.Transient)]
public class SendNotificationCommandConsumer :
    ACommandConsumer<SendNotificationCommand, IResponse>
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly ICommandExecutor _commandExecutor;

    public SendNotificationCommandConsumer(
        IHubConnectionService hubConnectionService,
        ICommandExecutor commandExecutor)
    {
        _hubConnectionService = hubConnectionService;
        _commandExecutor = commandExecutor;
    }

    protected override async Task<IResponse> OnConsumeAsync(SendNotificationCommand command, IMessageContext<SendNotificationCommand>? context = null)
    {
        var receiver = command.Receiver!;

        var hubIdUserIdsMapper = new Dictionary<string, List<string>>();

        foreach (var receiverId in receiver.ReceiverIds)
        {
            var hubInstanceId = await _hubConnectionService.GetUserConnectedHubInstanceIdAsync(receiverId);

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

        return Response.Success();
    }

    private async Task PublishNotificationToConnectedHubAsync(string hubId, List<string> receiverIds, Chat.Domain.Shared.Entities.Notification notification)
    {
        var publishNotificationToConnectedHubCommand = new PublishNotificationToConnectedHubCommand
        {
            HubInstanceId = hubId,
            Notification = notification,
            ReceiverIds = receiverIds
        };

        await _commandExecutor.ExecuteAsync(publishNotificationToConnectedHubCommand);
    }

    private async Task SendNotificationToClientAsync(List<string> receiverIds, Chat.Domain.Shared.Entities.Notification notification)
    {
        var sendNotificationToClientCommand = new SendNotificationToClientCommand
        {
            Notification = notification,
            ReceiverIds = receiverIds
        };

        await _commandExecutor.ExecuteAsync(sendNotificationToClientCommand);
    }
}