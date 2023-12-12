using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Activity.Application.CommandHandlers;

public class UpdateLastSeenCommandConsumer : ACommandConsumer<UpdateLastSeenCommand>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public UpdateLastSeenCommandConsumer(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    protected override async Task<IResult> OnConsumeAsync(UpdateLastSeenCommand command, IMessageContext<UpdateLastSeenCommand>? context = null)
    {
        var lastSeenModel = 
            await _lastSeenRepository.GetLastSeenModelByUserIdAsync(command.UserId) ?? new LastSeenModel
        {
            Id = Guid.NewGuid().ToString(),
            UserId = command.UserId,
            IsActive = command.IsActive
        };

        lastSeenModel.LastSeenAt = DateTime.UtcNow;

        if (!await _lastSeenRepository.SaveAsync(lastSeenModel))
        {
            return Result.Error("Save Last Seen Model ErrorMessage");
        }
        
        var response = Result.Success("Last seen time set successfully");
        response.SetData("LastSeenAt", lastSeenModel.LastSeenAt);

        return response;
    }
}