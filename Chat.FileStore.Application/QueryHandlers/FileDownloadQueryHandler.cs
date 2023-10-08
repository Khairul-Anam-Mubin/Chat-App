using Chat.FileStore.Application.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.FileStore.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<FileDownloadQuery, QueryResponse>), ServiceLifetime.Singleton)]
public class FileDownloadQueryHandler : AQueryHandler<FileDownloadQuery>
{
    private readonly IFileRepository _fileRepository;
    public FileDownloadQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    protected override async Task<QueryResponse> OnHandleAsync(FileDownloadQuery query)
    {
        var response = query.CreateResponse();
        var fileModel = await _fileRepository.GetFileModelByIdAsync(query.FileId);
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