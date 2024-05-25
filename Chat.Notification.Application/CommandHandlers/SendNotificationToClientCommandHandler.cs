using Chat.Notification.Application.Commands;
using Chat.Notification.Application.Constants;
using Chat.Notification.Application.Helpers;
using Chat.Notification.Domain.Interfaces;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Notification.Application.CommandHandlers;

public class SendNotificationToClientCommandHandler : ICommandHandler<SendNotificationToClientCommand>
{
    private readonly INotificationHubService _notificationHubService;

    public SendNotificationToClientCommandHandler(INotificationHubService notificationHubService)
    {
        _notificationHubService = notificationHubService;
    }

    public Task<IResult> Handle(SendNotificationToClientCommand request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult> HandleAsync(SendNotificationToClientCommand request)
    {
        var notification = request.Notification;
        var receiverIds = request.ReceiverUserIds;

        foreach (var receiverId in receiverIds)
        {
            var groupId = NotificationGroupProvider.GetGroupByUserId(receiverId);

            await _notificationHubService.SendToGroupAsync(
                groupId, 
                notification, 
                NotificationClientMethods.NotificationReceived);
        }

        return Result.Success();
    }
}