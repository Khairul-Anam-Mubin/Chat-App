using Chat.Contact.Application.Interfaces;
using Chat.Contact.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<AcceptOrRejectContactRequestCommand, Response>), ServiceLifetime.Singleton)]
public class AcceptOrRejectContactRequestCommandHandler : IRequestHandler<AcceptOrRejectContactRequestCommand, Response>
{
    private readonly IContactRepository _contactRepository;

    public AcceptOrRejectContactRequestCommandHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Response> HandleAsync(AcceptOrRejectContactRequestCommand command)
    {
        var response = command.CreateResponse();

        var contact = await _contactRepository.GetByIdAsync(command.ContactId);
        if (contact == null)
        {
            throw new Exception("ContactModel not found");
        }

        if (command.IsAcceptRequest)
        {
            contact.IsPending = false;
            if (!await _contactRepository.SaveAsync(contact))
            {
                throw new Exception("ContactModel save problem");
            }
            response.Message = "ContactModel added";
        }
        else
        {
            if (!await _contactRepository.DeleteByIdAsync(command.ContactId))
            {
                throw new Exception("Delete contact problem");
            }
            response.Message = "ContactModel rejected";
        }

        return (Response)response;
    }
}