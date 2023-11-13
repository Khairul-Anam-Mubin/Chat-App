using Chat.Activity.Application.DTOs;
using Chat.Activity.Application.Extensions;
using Chat.Activity.Application.Queries;
using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application.QueryHandlers;

[ServiceRegister(typeof(IHandler<LastSeenQuery, LastSeenQueryResponse>), ServiceLifetime.Singleton)]
public class LastSeenQueryHandler : IHandler<LastSeenQuery, LastSeenQueryResponse>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public LastSeenQueryHandler(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    public async Task<LastSeenQueryResponse> HandleAsync(LastSeenQuery query)
    {
        var response = new LastSeenQueryResponse();

        var lastSeenModels = await _lastSeenRepository.GetLastSeenModelsByUserIdsAsync(query.UserIds);
        foreach (var lastSeenModel in lastSeenModels)
        {
            response.Items.Add(lastSeenModel.ToLastSeenDto());
        }

        return response;
    }
}