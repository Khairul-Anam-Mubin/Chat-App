using Chat.Contact.Application.Interfaces;
using Chat.Contact.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers
{
    [ServiceRegister(typeof(IRequestHandler<AcceptOrRejectContactRequestCommand, CommandResponse>), ServiceLifetime.Singleton)]
    public class AcceptOrRejectContactRequestCommandHandler : ACommandHandler<AcceptOrRejectContactRequestCommand>
    {
        private readonly IContactRepository _contactRepository;
        public AcceptOrRejectContactRequestCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        protected override async Task<CommandResponse> OnHandleAsync(AcceptOrRejectContactRequestCommand command)
        {
            var response = command.CreateResponse();
            var contact = await _contactRepository.GetContactByIdAsync(command.ContactId);
            if (contact == null)
            {
                throw new Exception("Contact not found");
            }
            if (command.IsAcceptRequest)
            {
                contact.IsPending = false;
                if (!await _contactRepository.SaveContactAsync(contact))
                {
                    throw new Exception("Contact save problem");
                }
                response.Message = "Contact added";
            }
            else
            {
                if (!await _contactRepository.DeleteContactById(command.ContactId))
                {
                    throw new Exception("Delete contact problem");
                }
                response.Message = "Contact rejected";
            }
            return response;
        }
    }
}