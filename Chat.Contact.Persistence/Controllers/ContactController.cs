using Chat.Contact.Application.Commands;
using Chat.Contact.Application.DTOs;
using Chat.Contact.Application.Queries;
using Chat.Framework.CQRS;
using Chat.Framework.RequestResponse;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Contact.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactController : ACommonController
{
    private readonly IQueryExecutor _queryExecutor;

    public ContactController(
        ICommandExecutor commandExecutor, 
        IQueryExecutor queryExecutor)
        : base(commandExecutor)
    {
        _queryExecutor = queryExecutor;
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
        return Ok(await _queryExecutor.ExecuteAsync<ContactQuery, IPaginationResponse<ContactDto>>(query));
    }
}