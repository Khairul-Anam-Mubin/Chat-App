using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;
using Chat.Notification.Application.Commands;
using Chat.Notification.Domain.Interfaces;

namespace Chat.Notification.Application.Consumers;

public class SendNotificationCommandConsumer : ACommandConsumer<SendNotificationCommand>
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

    protected override async Task<IResult> OnConsumeAsync(SendNotificationCommand command, IMessageContext<SendNotificationCommand>? context = null)
    {
        var receiver = command.Receiver;

        var notification = command.Notification;

        if (notification.IncludeSelf && !string.IsNullOrEmpty(notification.Sender))
        {
            receiver.ReceiverIds.Add(notification.Sender);
        }

        var hubIdUserIdsMapper = new Dictionary<string, HashSet<string>>();

        foreach (var receiverId in receiver.ReceiverIds)
        {
            var hubIds = await _hubConnectionService.GetUserConnectedHubIdsAsync(receiverId);

            foreach (var hubId in hubIds)
            {
                if (hubIdUserIdsMapper.TryGetValue(hubId, out var userIds))
                {
                    userIds.Add(receiverId);
                }
                else
                {
                    hubIdUserIdsMapper.Add(hubId, new HashSet<string> { receiverId });
                }
            }
        }

        foreach (var (hubId, userIds) in hubIdUserIdsMapper)
        {
            if (_hubConnectionService.GetCurrentHubId() == hubId)
            {
                await SendNotificationToClientAsync(userIds.ToList(), command.Notification!);
            }
            else
            {
                await PublishNotificationToConnectedHubAsync(hubId, userIds.ToList(), command.Notification!);
            }
        }

        return Result.Success();
    }

    private async Task PublishNotificationToConnectedHubAsync(string hubId, List<string> receiverIds, NotificationData notification)
    {
        var publishNotificationToConnectedHubCommand = new PublishNotificationToConnectedHubCommand
        {
            HubInstanceId = hubId,
            Notification = notification,
            ReceiverIds = receiverIds
        };

        await _commandExecutor.ExecuteAsync(publishNotificationToConnectedHubCommand);
    }

    private async Task SendNotificationToClientAsync(List<string> receiverIds, NotificationData notification)
    {
        var sendNotificationToClientCommand = new SendNotificationToClientCommand
        {
            Notification = notification,
            ReceiverIds = receiverIds
        };

        await _commandExecutor.ExecuteAsync(sendNotificationToClientCommand);
    }
}