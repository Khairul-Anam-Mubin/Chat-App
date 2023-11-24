using Chat.Activity.Application.Extensions;
using Chat.Activity.Application.Queries;
using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Framework.Mediators;

namespace Chat.Activity.Application.QueryHandlers;

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