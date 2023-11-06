using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<UpdateLastSeenCommand, Response>), ServiceLifetime.Singleton)]
public class UpdateLastSeenCommandConsumer : ACommandConsumer<UpdateLastSeenCommand, Response>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public UpdateLastSeenCommandConsumer(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    protected override async Task<Response> OnConsumeAsync(UpdateLastSeenCommand command, IMessageContext<UpdateLastSeenCommand>? context = null)
    {
        var response = command.CreateResponse();

        var lastSeenModel = await _lastSeenRepository.GetLastSeenModelByUserIdAsync(command.UserId) ?? new LastSeenModel
        {
            Id = Guid.NewGuid().ToString(),
            UserId = command.UserId,
            IsActive = command.IsActive
        };

        lastSeenModel.LastSeenAt = DateTime.UtcNow;

        if (!await _lastSeenRepository.SaveAsync(lastSeenModel))
        {
            response.SetErrorMessage("Save Last Seen Model Error");
            return (Response)response;
        }

        response.SetSuccessMessage("Last seen time set successfully");
        response.SetData("LastSeenAt", lastSeenModel.LastSeenAt);

        return (Response)response;
    }
}