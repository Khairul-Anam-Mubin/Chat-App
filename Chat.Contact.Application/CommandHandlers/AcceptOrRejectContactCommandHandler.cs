using Chat.Contacts.Application.Commands;
using Chat.Contacts.Domain.Repositories;
using Chat.Contacts.Domain.Results;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contacts.Application.CommandHandlers;

public class AcceptOrRejectContactCommandHandler : ICommandHandler<AcceptOrRejectContactRequestCommand>
{
    private readonly IContactRepository _contactRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public AcceptOrRejectContactCommandHandler(IContactRepository contactRepository, IScopeIdentity scopeIdentity)
    {
        _contactRepository = contactRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult> HandleAsync(AcceptOrRejectContactRequestCommand command)
    {
        var contact = await _contactRepository.GetByIdAsync(command.ContactId);

        if (contact is null)
        {
            return Result.Error().ContactNotFound();
        }

        var userId = _scopeIdentity.GetUserId();

        if (command.IsAcceptRequest)
        {
            var acceptResult = contact.AcceptRequest(userId);

            if (acceptResult.IsFailure)
            {
                return acceptResult;
            }

            if (!await _contactRepository.SaveAsync(contact))
            {
                return Result.Error().ContactSaveProblem();
            }

            return acceptResult;
        }

        var rejectResult = contact.RejectRequest(userId);

        if (rejectResult.IsFailure) return rejectResult;

        if (!await _contactRepository.DeleteByIdAsync(contact.Id))
        {
            return Result.Error().ContactDeleteProblem();
        }

        return rejectResult;
    }
}