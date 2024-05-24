using Chat.FileStore.Application.DTOs;
using Chat.FileStore.Application.Queries;
using Chat.FileStore.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;
using Peacious.Framework.Results;

namespace Chat.FileStore.Application.QueryHandlers;

public class FileDownloadQueryHandler : IQueryHandler<FileDownloadQuery, IPaginationResponse<FileDownloadResult>>
{
    private readonly IFileDirectoryRepository _fileDirectoryRepository;

    public FileDownloadQueryHandler(IFileDirectoryRepository fileDirectoryRepository)
    {
        _fileDirectoryRepository = fileDirectoryRepository;
    }

    public async Task<IResult<IPaginationResponse<FileDownloadResult>>> HandleAsync(FileDownloadQuery query)
    {
        var response = query.CreateResponse();

        var fileDirectory = await _fileDirectoryRepository.GetByIdAsync(query.FileId);

        if (fileDirectory is null)
        {
            return Result.Error(response, "File not found");
        }

        var path = fileDirectory.Url;

        var fileDownloadResult = new FileDownloadResult
        {
            FileDirectory = fileDirectory,
            ContentType = GetContentType(fileDirectory.Extension)
        };

        await using (var fileStream = new FileStream(path, FileMode.Open))
        {
            fileDownloadResult.FileBytes = new byte[fileStream.Length];
            _ = await fileStream.ReadAsync(fileDownloadResult.FileBytes, 0, fileDownloadResult.FileBytes.Length);
        }

        response.AddItem(fileDownloadResult);

        return Result.Success(response);
    }

    private string GetContentType(string fileExtension)
    {
        fileExtension = fileExtension.Replace(".", "");
        return $"image/{fileExtension}";
    }
}