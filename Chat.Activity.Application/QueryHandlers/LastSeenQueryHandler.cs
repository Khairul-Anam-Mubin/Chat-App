using Chat.Activity.Application.DTOs;
using Chat.Activity.Application.Extensions;
using Chat.Activity.Application.Queries;
using Chat.Activity.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Activity.Application.QueryHandlers;

public class LastSeenQueryHandler : IQueryHandler<LastSeenQuery, List<LastSeenDto>>
{
    private readonly ILastSeenRepository _lastSeenRepository;

    public LastSeenQueryHandler(ILastSeenRepository lastSeenRepository)
    {
        _lastSeenRepository = lastSeenRepository;
    }

    public async Task<IResult<List<LastSeenDto>>> HandleAsync(LastSeenQuery query)
    {
        var lastSeenModels = 
            await _lastSeenRepository.GetLastSeenModelsByUserIdsAsync(query.UserIds);
        
        var lastSeenModelDtoList = new List<LastSeenDto>();

        foreach (var lastSeenModel in lastSeenModels)
        {
            lastSeenModelDtoList.Add(lastSeenModel.ToLastSeenDto());
        }

        return Result.Success(lastSeenModelDtoList);
    }
}