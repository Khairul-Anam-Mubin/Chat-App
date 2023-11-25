using Chat.FileStore.Application.Queries;
using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;

namespace Chat.FileStore.Application.QueryHandlers;

public class FileModelQueryHandler : IHandler<FileModelQuery, IPaginationResponse<FileModel>>
{
    private readonly IFileRepository _fileRepository;

    public FileModelQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IPaginationResponse<FileModel>> HandleAsync(FileModelQuery query)
    {
        var response = query.CreateResponse();

        var fileModel = await _fileRepository.GetByIdAsync(query.FileId);
        if (fileModel == null)
        {
            response.ErrorMessage("File not found");
            return response;
        }

        var path = Directory.GetCurrentDirectory();
        fileModel.Url = Path.Combine(path, fileModel.Url);

        response.AddItem(fileModel);

        return response;
    }
}