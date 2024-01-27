using Chat.Activity.Domain.Interfaces.Repositories;
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
        if (!await _lastSeenRepository.TrackLastSeenAsync(command.UserId, command.IsActive))
        {
            return Result.Error("Last Seen Model Update Failed");
        }
        
        return Result.Success();
    }
}