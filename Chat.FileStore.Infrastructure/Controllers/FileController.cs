using Chat.FileStore.Application.Commands;
using Chat.FileStore.Application.DTOs;
using Chat.FileStore.Application.Queries;
using Chat.FileStore.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;
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
    private readonly IQueryExecutor _queryExecutor;

    public FileController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : base(commandExecutor)
    {
        _queryExecutor = queryExecutor;
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
        var result = await _queryExecutor.ExecuteAsync<FileDownloadQuery, IPaginationResponse<FileDownloadResult>>(query);

        var response = result.Response;

        if (response is null)
        {
            return NotFound();
        }

        var fileDownloadResult = response.Items.FirstOrDefault();

        return File(fileDownloadResult?.FileBytes, fileDownloadResult?.ContentType);
    }

    [HttpPost]
    [Route("get")]
    public async Task<IActionResult> GetFileModelAsync(FileModelQuery query)
    {
        return Ok(await _queryExecutor.ExecuteAsync<FileModelQuery, IPaginationResponse<FileModel>>(query));
    }
}