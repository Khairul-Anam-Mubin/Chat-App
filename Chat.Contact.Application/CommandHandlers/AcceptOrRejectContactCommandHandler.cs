using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

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
            return Result.Error("Contact not found");
        }

        var userId = _scopeIdentity.GetUserId();

        if (command.IsAcceptRequest)
        {
            var result = contact.AcceptRequest(userId);

            if (result.IsFailure)
            {
                return result;
            }

            if (!await _contactRepository.SaveAsync(contact))
            {
                return Result.Error("Contact save problem");
            }

            return Result.Success("Contact Accepted");
        }

        if (!await _contactRepository.DeleteByIdAsync(command.ContactId))
        {
            return Result.Error("Delete contact problem");
        }

        return Result.Success("Contact rejected");
    }
}