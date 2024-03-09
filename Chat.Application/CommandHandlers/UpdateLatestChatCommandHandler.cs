using Chat.Application.Commands;
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
            latestChatModel = command.LatestChatModel;
            latestChatModel.Id = Guid.NewGuid().ToString();
            latestChatModel.Occurrence = 1;
        }

        latestChatModel.Message = command.LatestChatModel.Message;
        latestChatModel.SentAt = command.LatestChatModel.SentAt;
        latestChatModel.Status = command.LatestChatModel.Status;

        if (command.LatestChatModel.UserId == latestChatModel.UserId)
        {
            latestChatModel.Occurrence++;
        }
        else 
        {
            latestChatModel.Occurrence = 1;
            latestChatModel.UserId = command.LatestChatModel.UserId;
            latestChatModel.SendTo = command.LatestChatModel.SendTo;
        }

        await _latestChatRepository.SaveAsync(latestChatModel);

        return Result.Success();
    }
}