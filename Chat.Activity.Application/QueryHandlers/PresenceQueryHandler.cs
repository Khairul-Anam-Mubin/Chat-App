using Chat.Activity.Application.DTOs;
using Chat.Activity.Application.Extensions;
using Chat.Activity.Application.Queries;
using Chat.Activity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Activity.Application.QueryHandlers;

public class PresenceQueryHandler : IQueryHandler<PresenceQuery, List<PresenceDto>>
{
    private readonly IPresenceRepository _presenceRepository;

    public PresenceQueryHandler(IPresenceRepository presenceRepository)
    {
        _presenceRepository = presenceRepository;
    }

    public async Task<IResult<List<PresenceDto>>> HandleAsync(PresenceQuery query)
    {
        var presenceList = 
            await _presenceRepository.GetPresenceListByUserIdsAsync(query.UserIds);
        
        var presenceDtoList = new List<PresenceDto>();

        foreach (var presence in presenceList)
        {
            presenceDtoList.Add(presence.ToLastSeenDto());
        }

        return Result.Success(presenceDtoList);
    }
}