using Chat.Application.Interfaces;
using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<UpdateChatsStatusCommand, Response>), ServiceLifetime.Singleton)]
public class UpdateChatsStatusCommandHandler : 
    IRequestHandler<UpdateChatsStatusCommand, Response>
{
        
    private readonly ILatestChatRepository _latestChatRepository;
    private readonly IChatRepository _chatRepository;
        
    public UpdateChatsStatusCommandHandler(IChatRepository chatRepository, ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
        _chatRepository = chatRepository;
    }

    public async Task<Response> HandleAsync(UpdateChatsStatusCommand command)
    {
        var response = command.CreateResponse();

        var latestChatModel = await _latestChatRepository.GetLatestChatAsync(command.UserId, command.OpenedChatUserId);
        if (latestChatModel == null)
        {
            throw new Exception("LatestChatModel not found");
        }

        if (latestChatModel.UserId != command.UserId)
        {
            latestChatModel.Occurrence = 0;
            await _latestChatRepository.SaveAsync(latestChatModel);
        }
            
        var chatModels = await _chatRepository.GetSenderAndReceiverSpecificChatModelsAsync(command.OpenedChatUserId, command.UserId);
        foreach (var chatModel in chatModels)
        {
            chatModel.Status = "Seen";
        }

        await _chatRepository.SaveAsync(chatModels);

        return (Response)response;
    }
}