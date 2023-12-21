using Chat.FileStore.Application.Queries;
using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.FileStore.Application.QueryHandlers;

public class FileModelQueryHandler : IQueryHandler<FileModelQuery, IPaginationResponse<FileModel>>
{
    private readonly IFileRepository _fileRepository;

    public FileModelQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IResult<IPaginationResponse<FileModel>>> HandleAsync(FileModelQuery query)
    {
        var response = query.CreateResponse();

        var fileModel = await _fileRepository.GetByIdAsync(query.FileId);

        if (fileModel is null)
        {
            return Result.Error(response, "File not found");
        }

        response.AddItem(fileModel);

        return Result.Success(response);
    }
}