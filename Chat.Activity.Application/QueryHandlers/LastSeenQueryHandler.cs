using Chat.Activity.Application.Extensions;
using Chat.Activity.Application.Interfaces.Repositories;
using Chat.Activity.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<LastSeenQuery, QueryResponse>), ServiceLifetime.Singleton)]
public class LastSeenQueryHandler : AQueryHandler<LastSeenQuery>
{
    private readonly ILastSeenRepository _lastSeenRepository;
    public LastSeenQueryHandler(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    protected override async Task<QueryResponse> OnHandleAsync(LastSeenQuery query)
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