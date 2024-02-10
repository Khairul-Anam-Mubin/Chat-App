using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
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
        ICommandExecutor commandExecutor,
        IScopeIdentity scopeIdentity) : base (scopeIdentity)
    {
        _hubConnectionService = hubConnectionService;
        _commandExecutor = commandExecutor;
    }

    protected override async Task<IResult> OnConsumeAsync(SendNotificationCommand command, IMessageContext<SendNotificationCommand>? context = null)
    {
        var notification = command.Notification;
        var receiverUserIds = command.ReceiverUserIds;

        var hubIdUserIdsMapper = new Dictionary<string, HashSet<string>>();

        foreach (var receiverUserId in receiverUserIds)
        {
            var hubIds = await _hubConnectionService.GetUserConnectedHubIdsAsync(receiverUserId);

            foreach (var hubId in hubIds)
            {
                if (hubIdUserIdsMapper.TryGetValue(hubId, out var userIds))
                {
                    userIds.Add(receiverUserId);
                }
                else
                {
                    hubIdUserIdsMapper.Add(hubId, new HashSet<string> { receiverUserId });
                }
            }
        }

        foreach (var (hubId, userIds) in hubIdUserIdsMapper)
        {
            if (_hubConnectionService.GetCurrentHubId() == hubId)
            {
                await SendNotificationToClientAsync(userIds.ToList(), notification);
            }
            else
            {
                await PublishNotificationToConnectedHubAsync(hubId, userIds.ToList(), notification);
            }
        }

        return Result.Success();
    }

    private async Task PublishNotificationToConnectedHubAsync(string hubId, List<string> receiverUserIds, NotificationData notification)
    {
        var publishNotificationToConnectedHubCommand =
            new PublishNotificationToConnectedHubCommand(hubId, notification, receiverUserIds);

        await _commandExecutor.ExecuteAsync(publishNotificationToConnectedHubCommand);
    }

    private async Task SendNotificationToClientAsync(List<string> receiverUserIds, NotificationData notification)
    {
        var sendNotificationToClientCommand = 
            new SendNotificationToClientCommand(notification, receiverUserIds);

        await _commandExecutor.ExecuteAsync(sendNotificationToClientCommand);
    }
}