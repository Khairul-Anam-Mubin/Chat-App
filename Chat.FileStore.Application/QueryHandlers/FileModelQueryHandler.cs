using Chat.FileStore.Application.Interfaces;
using Chat.FileStore.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<FileModelQuery, QueryResponse>), ServiceLifetime.Singleton)]
public class FileModelQueryHandler : AQueryHandler<FileModelQuery, QueryResponse>
{
    private readonly IFileRepository _fileRepository;

    public FileModelQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    protected override async Task<QueryResponse> OnHandleAsync(FileModelQuery query)
    {
        var response = query.CreateResponse();

        var fileModel = await _fileRepository.GetByIdAsync(query.FileId);
        if (fileModel == null)
        {
            throw new Exception("File not found");
        }

        var path = Directory.GetCurrentDirectory();
        fileModel.Url = Path.Combine(path, fileModel.Url);

        response.AddItem(fileModel);

        return response;
    }
}