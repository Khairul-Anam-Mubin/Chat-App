using Chat.FileStore.Application.Commands;
using Chat.FileStore.Domain.Entities;
using Chat.FileStore.Domain.Repositories;
using Chat.FileStore.Domain.Results;
using Peacious.Framework.CQRS;
using Peacious.Framework.Extensions;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;
using Microsoft.Extensions.Configuration;

namespace Chat.FileStore.Application.CommandHandlers;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, string>
{
    private readonly IFileDirectoryRepository _fileDirectoryRepository;
    private readonly IConfiguration _configuration;
    private readonly IScopeIdentity _scopeIdentity;
    
    public UploadFileCommandHandler(
        IFileDirectoryRepository fileDirectoryRepository, 
        IConfiguration configuration,
        IScopeIdentity scopeIdentity)
    {
        _fileDirectoryRepository = fileDirectoryRepository;
        _configuration = configuration;
        _scopeIdentity = scopeIdentity;
    }

    public Task<IResult<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult<string>> HandleAsync(UploadFileCommand command)
    {
        var file = command.FormFile;
        var pathToSave = _configuration.TryGetConfig<string>("FileStorePath");
        var fileName = file.FileName;
        var fileId = Guid.NewGuid().ToString();
        var extension = Path.GetExtension(fileName);
        var fullPath = Path.Combine(pathToSave, fileId + extension);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileDirectoryCreateResult =
           FileDirectory.Create(fileId, fileName, extension, fullPath, _scopeIdentity.GetUserId());
        
        if (fileDirectoryCreateResult.IsFailure || fileDirectoryCreateResult.Value is null)
        {
            return (IResult<string>)fileDirectoryCreateResult;
        }

        var fileDirectory = fileDirectoryCreateResult.Value;
        
        if (!await _fileDirectoryRepository.SaveAsync(fileDirectory))
        {
            return (IResult<string>)Error.SaveProblem<FileDirectory>();
        }

        return Result.Success<string>().FileUpload(fileDirectory.Id);
    }
}