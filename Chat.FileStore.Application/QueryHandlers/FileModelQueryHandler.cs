using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<FileModelQuery, IQueryResponse>), ServiceLifetime.Singleton)]
public class FileModelQueryHandler : IRequestHandler<FileModelQuery, IQueryResponse>
{
    private readonly IFileRepository _fileRepository;

    public FileModelQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IQueryResponse> HandleAsync(FileModelQuery query)
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