using Chat.Application.Commands;
using Chat.Application.Extensions;
using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Entities;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IHandler<SendMessageCommand, IResponse>), 
    ServiceLifetime.Transient)]
public class SendMessageCommandHandler : 
    IHandler<SendMessageCommand, IResponse>
{
    private readonly IChatRepository _chatRepository;
    private readonly ICommandExecutor _commandExecutor;
    private readonly ICommandBus _commandBus;

    public SendMessageCommandHandler(
        IChatRepository chatRepository,
        ICommandExecutor commandExecutor,
        ICommandBus commandBus)
    {
        _chatRepository = chatRepository;
        _commandExecutor = commandExecutor;
        _commandBus = commandBus;
    }

    public async Task<IResponse> HandleAsync(SendMessageCommand command)
    { 
        var chatModel = command.ChatModel;

        if (chatModel is null)
        {
            throw new Exception("ChatModel not set");
        }

        chatModel.Id = Guid.NewGuid().ToString();
        chatModel.SentAt = DateTime.UtcNow;
        chatModel.Status = "Sent";

        if (!await _chatRepository.SaveAsync(chatModel))
        {
            throw new Exception("ChatModel Creation Failed");
        }

        await SendChatNotificationAsync(chatModel);

        await UpdateLatestChatModelAsync(chatModel);
        
        var response = Response.Success();
        
        response.SetData("Message", chatModel.ToChatDto());

        return response;
    }

    private Task SendChatNotificationAsync(ChatModel chatModel)
    {
        var sendNotificationCommand = new SendNotificationCommand()
        {
            Notification = new Notification
            {
                Id = chatModel.Id,
                Content = chatModel,
                ContentType = "ChatModel",
                Sender = chatModel.UserId,
                NotificationType = NotificationType.UserChat
            },
            Receiver = new NotificationReceiver
            {
                ReceiverIds = new List<string> { chatModel.SendTo }
            }
        };

        return _commandBus.SendAsync(sendNotificationCommand);
    }

    private Task UpdateLatestChatModelAsync(ChatModel chatModel)
    {
        var latestChatModel = chatModel.ToLatestChatModel();

        var updateLatestChatCommand = new UpdateLatestChatCommand
        {
            LatestChatModel = latestChatModel
        };

        return _commandExecutor.ExecuteAsync(updateLatestChatCommand);
    }
}