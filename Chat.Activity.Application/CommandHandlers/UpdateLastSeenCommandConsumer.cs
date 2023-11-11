using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application.CommandHandlers;

[ServiceRegister(typeof(IHandler<UpdateLastSeenCommand, IResponse>), ServiceLifetime.Singleton)]
public class UpdateLastSeenCommandConsumer : ACommandConsumer<UpdateLastSeenCommand, IResponse>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public UpdateLastSeenCommandConsumer(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    protected override async Task<IResponse> OnConsumeAsync(UpdateLastSeenCommand command, IMessageContext<UpdateLastSeenCommand>? context = null)
    {
        var lastSeenModel = await _lastSeenRepository.GetLastSeenModelByUserIdAsync(command.UserId) ?? new LastSeenModel
        {
            Id = Guid.NewGuid().ToString(),
            UserId = command.UserId,
            IsActive = command.IsActive
        };

        lastSeenModel.LastSeenAt = DateTime.UtcNow;

        if (!await _lastSeenRepository.SaveAsync(lastSeenModel))
        {
            return Response.Error("Save Last Seen Model ErrorMessage");
        }
        
        var response = Response.Success("Last seen time set successfully");
        response.SetData("LastSeenAt", lastSeenModel.LastSeenAt);

        return response;
    }
}