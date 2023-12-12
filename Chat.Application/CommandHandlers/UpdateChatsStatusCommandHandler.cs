using Chat.Application.Commands;
using Chat.Domain.Interfaces;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class UpdateChatsStatusCommandHandler : 
    ICommandHandler<UpdateChatsStatusCommand>
{
        
    private readonly ILatestChatRepository _latestChatRepository;
    private readonly IChatRepository _chatRepository;
        
    public UpdateChatsStatusCommandHandler(IChatRepository chatRepository, ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
        _chatRepository = chatRepository;
    }

    public async Task<IResult> HandleAsync(UpdateChatsStatusCommand command)
    {
        var latestChatModel = await _latestChatRepository.GetLatestChatAsync(command.UserId, command.OpenedChatUserId);
        if (latestChatModel == null)
        {
            return Result.Error("LatestChatModel not found");
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

        return Result.Success();
    }
}