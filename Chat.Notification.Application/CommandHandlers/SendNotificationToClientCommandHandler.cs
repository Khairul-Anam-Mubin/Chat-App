using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Notification.Application.Commands;
using Chat.Notification.Domain.Interfaces;

namespace Chat.Notification.Application.CommandHandlers;

public class SendNotificationToClientCommandHandler : ICommandHandler<SendNotificationToClientCommand>
{
    private readonly INotificationHubService _notificationHubService;
    private readonly IHubConnectionService _hubConnectionService;

    public SendNotificationToClientCommandHandler(
        INotificationHubService notificationHubService, 
        IHubConnectionService hubConnectionService)
    {
        _notificationHubService = notificationHubService;
        _hubConnectionService = hubConnectionService;
    }

    public async Task<IResult> HandleAsync(SendNotificationToClientCommand request)
    {
        var notification = request.Notification;
        var receiverIds = request.ReceiverIds;

        foreach (var receiverId in receiverIds)
        {
            var connectionIds = _hubConnectionService.GetConnectionIds(receiverId);
            await _notificationHubService.SendToConnectionsAsync(connectionIds, notification, notification!.NotificationType.ToString());
        }

        return Result.Success();
    }
}