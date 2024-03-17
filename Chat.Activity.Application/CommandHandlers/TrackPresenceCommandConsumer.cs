using Chat.Activity.Domain.Repositories;
using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Activity.Application.CommandHandlers;

public class TrackPresenceCommandConsumer : ACommandConsumer<TrackPresenceCommand>
{
    private readonly IPresenceRepository _presenceRepository;

    public TrackPresenceCommandConsumer(IPresenceRepository presenceRepository, IScopeIdentity scopeIdentity) 
        : base(scopeIdentity)
    {
        _presenceRepository = presenceRepository;
    }

    protected override async Task<IResult> OnConsumeAsync(TrackPresenceCommand command, IMessageContext<TrackPresenceCommand>? context = null)
    {
        if (!await _presenceRepository.TrackPresenceAsync(command.UserId, command.IsActive))
        {
            return Result.Error("Track presence failed.");
        }
        
        return Result.Success();
    }
}