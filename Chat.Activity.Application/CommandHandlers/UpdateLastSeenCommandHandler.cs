using Chat.Activity.Application.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Domain.Shared.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application.CommandHandlers
{
    [ServiceRegister(typeof(IRequestHandler<UpdateLastSeenCommand, CommandResponse>), ServiceLifetime.Singleton)]
    public class UpdateLastSeenCommandHandler : ACommandHandler<UpdateLastSeenCommand>
    {
        private readonly ILastSeenRepository _lastSeenRepository;

        public UpdateLastSeenCommandHandler(ILastSeenRepository lastSeenRepository)
        {
            _lastSeenRepository = lastSeenRepository;
        }

        protected override async Task<CommandResponse> OnHandleAsync(UpdateLastSeenCommand command)
        {
            var response = command.CreateResponse();
            var lastSeenModel = await _lastSeenRepository.GetLastSeenModelByUserIdAsync(command.UserId) ?? new LastSeenModel 
            {
                Id = Guid.NewGuid().ToString(),
                UserId = command.UserId,
                IsActive = command.IsActive
            };
            lastSeenModel.LastSeenAt = DateTime.UtcNow;
            if (!await _lastSeenRepository.SaveLastSeenModelAsync(lastSeenModel))
            {
                response.SetErrorMessage("Save Last Seen Model Error");
                return response;
            }
            response.SetSuccessMessage("Last seen time set successfully");
            response.SetData("LastSeenAt", lastSeenModel.LastSeenAt);
            return response;
        }
    }
}