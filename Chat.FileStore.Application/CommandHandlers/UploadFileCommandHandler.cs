using Chat.FileStore.Application.Commands;
using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.Identity;
using Chat.Framework.Results;
using Microsoft.Extensions.Configuration;

namespace Chat.FileStore.Application.CommandHandlers;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, string>
{
    private readonly IFileRepository _fileRepository;
    private readonly IConfiguration _configuration;
    private readonly IScopeIdentity _scopeIdentity;
    
    public UploadFileCommandHandler(
        IFileRepository fileRepository, 
        IConfiguration configuration,
        IScopeIdentity scopeIdentity)
    {
        _fileRepository = fileRepository;
        _configuration = configuration;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<string>> HandleAsync(UploadFileCommand command)
    {
        var file = command.FormFile;
        var pathToSave = _configuration.TryGetConfig<string>("FileStorePath");
        
        if (file.Length <= 0)
        {
            return Result.Error<string>("File Length 0");
        }

        var fileName = file.FileName;
        var fileId = Guid.NewGuid().ToString();
        var extension = Path.GetExtension(fileName);
        var fullPath = Path.Combine(pathToSave, fileId + extension);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileModel = new FileModel
        {
            Id = fileId,
            Extension = extension,
            Url = fullPath,
            UploadedAt = DateTime.UtcNow,
            Name = fileName,
            UserId = _scopeIdentity.GetUserId()!
        };
        
        if (!await _fileRepository.SaveAsync(fileModel))
        {
            return Result.Error<string>("File Save error to db");
        }
        
        return Result.Success(fileModel.Id,"File uploaded successfully");
    }
}