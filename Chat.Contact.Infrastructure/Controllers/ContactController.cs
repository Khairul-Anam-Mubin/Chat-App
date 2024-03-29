using Chat.Contacts.Application.Commands;
using Chat.Contacts.Application.DTOs;
using Chat.Contacts.Application.Queries;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Contacts.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactController : ACommonController
{
    public ContactController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        : base(commandExecutor, queryExecutor) { }

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
        return Ok(await GetQueryResponseAsync<ContactQuery, IPaginationResponse<ContactDto>>(query));
    }
}