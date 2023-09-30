using Chat.Domain.Shared.Queries;
using Chat.Framework.Proxy;
using Chat.Identity.Domain.Commands;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Identity.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ACommonController
    {
        public UserController(ICommandQueryProxy commandQueryProxy) : base(commandQueryProxy)
        {

        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync(RegisterCommand command)
        {
            return Ok(await GetCommandResponseAsync(command));
        }

        [HttpPost]
        [Route("profiles")]
        public async Task<IActionResult> UserProfileAsync(UserProfileQuery query)
        {
            return Ok(await GetQueryResponseAsync(query));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserProfileCommand command)
        {
            return Ok(await GetCommandResponseAsync(command));
        }
    }
}
