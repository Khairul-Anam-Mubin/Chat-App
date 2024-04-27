using Chat.Domain.Shared.Events;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Repositories;
using KCluster.Framework.CQRS;
using KCluster.Framework.EDD;
using KCluster.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class VerifyAccountCommandHandler : ICommandHandler<VerifyAccountCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventService _eventService;

    public VerifyAccountCommandHandler(IUserRepository userRepository, IEventService eventService)
    {
        _userRepository = userRepository;
        _eventService = eventService;
    }

    public async Task<IResult> HandleAsync(VerifyAccountCommand request)
    {
        if (!await _userRepository.UpdateEmailVerificationStatus(request.UserId, true))
        {
            return Result.Error("Account verification failed");
        }
        
        var verifiedUserAccountCreatedEvent = 
            new UserEmailVerifiedEvent(request.UserId);
        
        await _eventService.PublishIntegrationEventAsync(verifiedUserAccountCreatedEvent);
        
        return Result.Success("Account verified successfully.");
    }
}