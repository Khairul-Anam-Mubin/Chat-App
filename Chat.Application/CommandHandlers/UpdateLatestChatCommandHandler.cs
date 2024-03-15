using Chat.Application.Commands;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class UpdateLatestChatCommandHandler : ICommandHandler<UpdateLatestChatCommand>
{
    private readonly ILatestChatRepository _latestChatRepository;
        
    public UpdateLatestChatCommandHandler(ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
    }

    public async Task<IResult> HandleAsync(UpdateLatestChatCommand command)
    {
        var latestChatModel = 
            await _latestChatRepository.GetLatestChatAsync(command.LatestChatModel!.UserId, command.LatestChatModel.SendTo);
        
        if (latestChatModel is null)
        {
            latestChatModel = 
                LatestChatModel.Create(
                    command.LatestChatModel.Id, 
                    command.LatestChatModel.UserId, 
                    command.LatestChatModel.SendTo, 
                    command.LatestChatModel.Message, 
                    command.LatestChatModel.SentAt, 
                    command.LatestChatModel.Status, 
                    command.LatestChatModel.IsGroupMessage);
        }

        latestChatModel.Update(
            command.LatestChatModel.UserId,
            command.LatestChatModel.SendTo,
            command.LatestChatModel.Message,
            command.LatestChatModel.Status,
            command.LatestChatModel.SentAt);

        await _latestChatRepository.SaveAsync(latestChatModel);

        return Result.Success();
    }
}