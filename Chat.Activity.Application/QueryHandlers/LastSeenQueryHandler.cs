using Chat.Activity.Application.DTOs;
using Chat.Activity.Application.Extensions;
using Chat.Activity.Application.Queries;
using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application.QueryHandlers;

[ServiceRegister(typeof(IHandler<LastSeenQuery, IPaginationResponse<LastSeenDto>>), ServiceLifetime.Singleton)]
public class LastSeenQueryHandler : IHandler<LastSeenQuery, IPaginationResponse<LastSeenDto>>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public LastSeenQueryHandler(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    public async Task<IPaginationResponse<LastSeenDto>> HandleAsync(LastSeenQuery query)
    {
        var response = query.CreateResponse();

        var lastSeenModels = await _lastSeenRepository.GetLastSeenModelsByUserIdsAsync(query.UserIds);
        foreach (var lastSeenModel in lastSeenModels)
        {
            response.AddItem(lastSeenModel.ToLastSeenDto());
        }

        return response;
    }
}