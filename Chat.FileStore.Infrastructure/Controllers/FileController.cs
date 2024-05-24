using Chat.FileStore.Application.Commands;
using Chat.FileStore.Application.DTOs;
using Chat.FileStore.Application.Queries;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.FileStore.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ACommonController
{
    public FileController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) 
        : base(commandExecutor, queryExecutor) {}

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadAsync(IFormFile formFile)
    {
        var fileUploadCommand = new UploadFileCommand
        {
            FormFile = formFile
        };
        return Ok(await GetCommandResponseAsync<UploadFileCommand, string>(fileUploadCommand));
    }

    [HttpGet]
    [Route("download")]
    public async Task<IActionResult> DownloadAsync([FromQuery] string fileId)
    {
        var query = new FileDownloadQuery
        {
            FileId = fileId
        };
        var result = await GetQueryResponseAsync<FileDownloadQuery, IPaginationResponse<FileDownloadResult>>(query);

        var response = result.Value;

        if (response is null)
        {
            return NotFound();
        }

        var fileDownloadResult = response.Items.FirstOrDefault();

        if (fileDownloadResult == null) 
        {
            return NotFound();
        }

        return File(fileDownloadResult.FileBytes, fileDownloadResult.ContentType);
    }

    [HttpPost]
    [Route("get")]
    public async Task<IActionResult> GetFileModelAsync(FileDirectoryQuery query)
    {
        return base.Ok(await GetQueryResponseAsync<FileDirectoryQuery, IPaginationResponse<Domain.Entities.FileDirectory>>(query));
    }
}