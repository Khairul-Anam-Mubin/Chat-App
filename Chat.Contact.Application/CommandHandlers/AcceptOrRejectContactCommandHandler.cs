using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;

namespace Chat.Contact.Application.CommandHandlers;

public class AcceptOrRejectContactCommandHandler : 
    IHandler<AcceptOrRejectContactRequestCommand, IResponse>
{
    private readonly IContactRepository _contactRepository;

    public AcceptOrRejectContactCommandHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IResponse> HandleAsync(AcceptOrRejectContactRequestCommand command)
    {
        var response = Response.Success();

        var contact = await _contactRepository.GetByIdAsync(command.ContactId);
        if (contact == null)
        {
            return Response.Error("Contact not found");
        }

        if (command.IsAcceptRequest)
        {
            contact.IsPending = false;
            if (!await _contactRepository.SaveAsync(contact))
            {
                return Response.Error("Contact save problem");
            }
            response.Message = "Contact added";
        }
        else
        {
            if (!await _contactRepository.DeleteByIdAsync(command.ContactId))
            {
                return Response.Error("Delete contact problem");
            }
            response.Message = "Contact rejected";
        }

        return response;
    }
}