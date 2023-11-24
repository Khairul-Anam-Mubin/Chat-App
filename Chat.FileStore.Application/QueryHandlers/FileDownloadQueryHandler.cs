using Chat.FileStore.Application.DTOs;
using Chat.FileStore.Application.Queries;
using Chat.FileStore.Domain.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;

namespace Chat.FileStore.Application.QueryHandlers;

public class FileDownloadQueryHandler : IHandler<FileDownloadQuery, IPaginationResponse<FileDownloadResult>>
{
    private readonly IFileRepository _fileRepository;

    public FileDownloadQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IPaginationResponse<FileDownloadResult>> HandleAsync(FileDownloadQuery query)
    {
        var response = query.CreateResponse();

        var fileModel = await _fileRepository.GetByIdAsync(query.FileId);
        if (fileModel == null)
        {
            throw new Exception("File not found");
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), fileModel.Url);

        var fileDownloadResult = new FileDownloadResult
        {
            FileModel = fileModel,
            ContentType = GetContentType(fileModel.Extension)
        };

        await using (var fileStream = new FileStream(path, FileMode.Open))
        {
            fileDownloadResult.FileBytes = new byte[fileStream.Length];
            await fileStream.ReadAsync(fileDownloadResult.FileBytes, 0, fileDownloadResult.FileBytes.Length);
        }

        response.AddItem(fileDownloadResult);

        return response;
    }

    private string GetContentType(string fileExtension)
    {
        fileExtension = fileExtension.Replace(".", "");
        return $"image/{fileExtension}";
    }
}