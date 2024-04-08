using Chat.Activity.Domain.Repositories;
using Chat.Activity.Domain.Results;
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
        var userId = ScopeIdentity.GetUserId()!;

        if (!await _presenceRepository.TrackPresenceAsync(userId))
        {
            return Result.Error().TrackPresenceFailed();
        }
        
        return Result.Success();
    }
}