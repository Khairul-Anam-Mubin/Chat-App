using Chat.FileStore.Domain.Commands;
using Chat.FileStore.Domain.Models;
using Chat.FileStore.Domain.Queries;
using Chat.Framework.CQRS;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.FileStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ACommonController
{
    private readonly IQueryService _queryService;

    public FileController(ICommandService commandService, IQueryService queryService) : base(commandService)
    {
        _queryService = queryService;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadAsync(IFormFile formFile)
    {
        var fileUploadCommand = new UploadFileCommand
        {
            FormFile = formFile
        };
        return Ok(await GetCommandResponseAsync(fileUploadCommand));
    }

    [HttpGet]
    [Route("download")]
    public async Task<IActionResult> DownloadAsync([FromQuery] string fileId)
    {
        var query = new FileDownloadQuery
        {
            FileId = fileId
        };
        var response = await _queryService.GetResponseAsync<FileDownloadQuery, QueryResponse>(query);
        var fileDownloadResult = (FileDownloadResult)response.Items[0];
        return File(fileDownloadResult.FileBytes, fileDownloadResult.ContentType);
    }

    [HttpPost]
    [Route("get")]
    public async Task<IActionResult> GetFileModelAsync(FileModelQuery query)
    {
        return Ok(await _queryService.GetResponseAsync<FileModelQuery, QueryResponse>(query));
    }
}