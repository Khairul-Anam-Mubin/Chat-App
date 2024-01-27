using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class AcceptOrRejectContactCommandHandler : 
    ICommandHandler<AcceptOrRejectContactRequestCommand>
{
    private readonly IContactRepository _contactRepository;

    public AcceptOrRejectContactCommandHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IResult> HandleAsync(AcceptOrRejectContactRequestCommand command)
    {
        var contact = await _contactRepository.GetByIdAsync(command.ContactId);
        
        if (contact is null)
        {
            return Result.Error("Contact not found");
        }

        if (command.IsAcceptRequest)
        {
            contact.IsPending = false;

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