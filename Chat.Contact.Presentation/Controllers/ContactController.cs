using Chat.Contact.Domain.Commands;
using Chat.Contact.Domain.Queries;
using Chat.Framework.CQRS;
using Chat.Framework.Proxy;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Contact.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactController : ACommonController
{
    private readonly IQueryService _queryService;

    public ContactController(
        ICommandService commandService, 
        IQueryService queryService)
        : base(commandService)
    {
        _queryService = queryService;
    }

    [HttpPost, Route("add")]
    public async Task<IActionResult> AddContactAsync(AddContactCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("accept-reject")]
    public async Task<IActionResult> AcceptOrRejectContactRequestAsync(AcceptOrRejectContactRequestCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("get")]
    public async Task<IActionResult> AddContactAsync(ContactQuery query)
    {
        return Ok(await _queryService.GetResponseAsync<ContactQuery, QueryResponse>(query));
    }
}