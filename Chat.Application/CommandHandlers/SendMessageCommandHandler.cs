using Chat.Application.Extensions;
using Chat.Application.Interfaces;
using Chat.Domain.Commands;
using Chat.Domain.Events;
using Chat.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Events;
using Chat.Framework.Mediators;
using Chat.Framework.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<SendMessageCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class SendMessageCommandHandler : ACommandHandler<SendMessageCommand>
{
    private readonly IChatRepository _chatRepository;
    private readonly ICommandQueryProxy _commandQueryProxy;
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IEventPublisher _eventPublisher;

    public SendMessageCommandHandler(
        IChatRepository chatRepository,
        ICommandQueryProxy commandQueryProxy, 
        IHubConnectionService hubConnectionService, IEventPublisher eventPublisher)
    {
        _chatRepository = chatRepository;
        _commandQueryProxy = commandQueryProxy;
        _hubConnectionService = hubConnectionService;
        _eventPublisher = eventPublisher;
    }

    protected override async Task<CommandResponse> OnHandleAsync(SendMessageCommand command)
    { 
        var chatModel = command.ChatModel;

        if (chatModel is null)
        {
            throw new Exception("ChatModel not set");
        }

        chatModel.Id = Guid.NewGuid().ToString();
        chatModel.SentAt = DateTime.UtcNow;
        chatModel.Status = "Sent";

        await _chatRepository.SaveAsync(chatModel);

        if (_hubConnectionService.IsUserConnectedWithCurrentHubInstance(chatModel.SendTo))
        {
            await SendMessageToClientAsync(chatModel);
        }
        else if (await _hubConnectionService.IsUserConnectedWithAnyHubInstanceAsync(chatModel.SendTo))
        {
            await PublishMessageToConnectedHubAsync(chatModel);
        }

        await UpdateLatestChatModelAsync(chatModel);
        
        var response = command.CreateResponse();
        
        response.SetData("Message", chatModel.ToChatDto());

        return response;
    }

    private Task SendMessageToClientAsync(ChatModel chatModel)
    {
        var sendMessageToClientCommand = new SendMessageToClientCommand
        {
            ChatModel = chatModel,
            FireAndForget = true
        };

        return _commandQueryProxy.GetCommandResponseAsync(sendMessageToClientCommand);
    }

    private Task PublishMessageToConnectedHubAsync(ChatModel chatModel)
    {
        var publishMessageToConnectedServerEvent = new PublishMessageToConnectedHubEvent
        {
            UserId = chatModel.UserId,
            SendTo = chatModel.SendTo,
            MessageId = chatModel.Id,
            ChatModel = chatModel
        };

        return _eventPublisher.PublishAsync(publishMessageToConnectedServerEvent);
    }

    private Task UpdateLatestChatModelAsync(ChatModel chatModel)
    {
        var latestChatModel = chatModel.ToLatestChatModel();

        var updateLatestChatCommand = new UpdateLatestChatCommand
        {
            LatestChatModel = latestChatModel,
            FireAndForget = true
        };

        return _commandQueryProxy.GetCommandResponseAsync(updateLatestChatCommand);
    }
}