using Chat.Contact.Domain.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Framework.Attributes;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<AcceptOrRejectContactRequestCommand, IResponse>), ServiceLifetime.Singleton)]
public class AcceptOrRejectContactRequestCommandHandler : 
    IRequestHandler<AcceptOrRejectContactRequestCommand, IResponse>
{
    private readonly IContactRepository _contactRepository;

    public AcceptOrRejectContactRequestCommandHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IResponse> HandleAsync(AcceptOrRejectContactRequestCommand command)
    {
        var response = Response.Success();

        var contact = await _contactRepository.GetByIdAsync(command.ContactId);
        if (contact == null)
        {
            throw new Exception("Contact not found");
        }

        if (command.IsAcceptRequest)
        {
            contact.IsPending = false;
            if (!await _contactRepository.SaveAsync(contact))
            {
                throw new Exception("Contact save problem");
            }
            response.Message = "Contact added";
        }
        else
        {
            if (!await _contactRepository.DeleteByIdAsync(command.ContactId))
            {
                throw new Exception("Delete contact problem");
            }
            response.Message = "Contact rejected";
        }

        return response;
    }
}