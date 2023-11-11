using Chat.Domain.Commands;
using Chat.Domain.Interfaces;
using Chat.Framework.Attributes;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<UpdateLatestChatCommand, IResponse>), ServiceLifetime.Singleton)]
public class UpdateLatestChatCommandHandler : 
    IRequestHandler<UpdateLatestChatCommand, IResponse>
{
        
    private readonly ILatestChatRepository _latestChatRepository;
        
    public UpdateLatestChatCommandHandler(ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
    }

    public async Task<IResponse> HandleAsync(UpdateLatestChatCommand command)
    {
        var response = Response.Success();

        var latestChatModel = await _latestChatRepository.GetLatestChatAsync(command.LatestChatModel!.UserId, command.LatestChatModel.SendTo);
        if (latestChatModel == null)
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

        return response;
    }
}