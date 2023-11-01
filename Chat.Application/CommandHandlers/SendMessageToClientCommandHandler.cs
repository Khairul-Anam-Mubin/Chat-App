﻿using Chat.Application.Interfaces;
using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<SendMessageToClientCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class SendMessageToClientCommandHandler : IRequestHandler<SendMessageToClientCommand, CommandResponse>
{
    private readonly IChatHubService _chatHubService;
    private readonly IChatRepository _chatRepository;

    public SendMessageToClientCommandHandler(
        IChatHubService chatHubService,
        IChatRepository chatRepository)
    {
        _chatHubService = chatHubService;
        _chatRepository = chatRepository;
    }

    public async Task<CommandResponse> HandleAsync(SendMessageToClientCommand command)
    {
        var chatModel = command.ChatModel ?? await _chatRepository.GetByIdAsync(command.MessageId);
        
        if (chatModel == null)
        {
            throw new Exception("ChatModel not found");
        }

        await _chatHubService.SendAsync(chatModel.SendTo, chatModel);

        return command.CreateResponse();
    }
}