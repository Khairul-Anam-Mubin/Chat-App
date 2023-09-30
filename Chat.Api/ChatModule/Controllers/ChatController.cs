using Chat.Api.ChatModule.Commands;
using Chat.Api.ChatModule.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chat.Framework.Proxy;
using Chat.Presentation.Shared.Controllers;

namespace Chat.Api.ChatModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ACommonController
    {
        public ChatController(ICommandQueryProxy commandQueryProxy) 
            : base(commandQueryProxy)
        {
            
        }

        [HttpPost, Route("send")]
        public async Task<IActionResult> SendMessageAsync(SendMessageCommand command)
        {
            return Ok(await GetCommandResponseAsync(command));
        }

        [HttpPost, Route("update-status")]
        public async Task<IActionResult> UpdateChatsStatusAsync(UpdateChatsStatusCommand command)
        {
            return Ok(await GetCommandResponseAsync(command));
        }

        [HttpPost, Route("list")]
        public async Task<IActionResult> GetChatListAsync(ChatListQuery query)
        {
            return Ok(await GetQueryResponseAsync(query));
        }

        [HttpPost, Route("get")]
        public async Task<IActionResult> GetChatsAsync(ChatQuery query)
        {
            return Ok(await GetQueryResponseAsync(query));
        }
        
    }
}