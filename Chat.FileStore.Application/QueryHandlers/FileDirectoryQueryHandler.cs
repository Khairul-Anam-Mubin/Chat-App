using Chat.FileStore.Application.Queries;
using Chat.FileStore.Domain.Entities;
using Chat.FileStore.Domain.Repositories;
using KCluster.Framework.CQRS;
using KCluster.Framework.Pagination;
using KCluster.Framework.Results;

namespace Chat.FileStore.Application.QueryHandlers;

public class FileDirectoryQueryHandler : IQueryHandler<FileDirectoryQuery, IPaginationResponse<FileDirectory>>
{
    private readonly IFileDirectoryRepository _fileDirectoryRepository;

    public FileDirectoryQueryHandler(IFileDirectoryRepository fileDirectoryRespository)
    {
        _fileDirectoryRepository = fileDirectoryRespository;
    }

    public async Task<IResult<IPaginationResponse<FileDirectory>>> HandleAsync(FileDirectoryQuery query)
    {
        var response = query.CreateResponse();

        var fileDirectory = await _fileDirectoryRepository.GetByIdAsync(query.FileId);

        if (fileDirectory is null)
        {
            return Result.Error(response, "File not found");
        }

        response.AddItem(fileDirectory);

        return Result.Success(response);
    }
}