using Chat.Application.Shared.Providers;
using Chat.FileStore.Application.Commands;
using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Chat.FileStore.Application.CommandHandlers;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, string>
{
    private readonly IFileRepository _fileRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public UploadFileCommandHandler(
        IFileRepository fileRepository, 
        IHttpContextAccessor httpContextAccessor, 
        IConfiguration configuration)
    {
        _fileRepository = fileRepository;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<IResult<string>> HandleAsync(UploadFileCommand command)
    {
        var file = command.FormFile;
        var pathToSave = _configuration.TryGetConfig<string>("FileStorePath");
        
        if (file.Length <= 0)
        {
            return Result<string>.Error("File Length 0");
        }

        var fileName = file.FileName;
        var fileId = Guid.NewGuid().ToString();
        var extension = Path.GetExtension(fileName);
        var fullPath = Path.Combine(pathToSave, fileId + extension);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var requestContextFromAccessor = _httpContextAccessor.HttpContext;
        var currentUser = IdentityProvider.GetUserProfile(requestContextFromAccessor?.GetAccessToken());
        
        var fileModel = new FileModel
        {
            Id = fileId,
            Extension = extension,
            Url = fullPath,
            UploadedAt = DateTime.UtcNow,
            Name = fileName,
            UserId = currentUser?.Id ?? ""
        };
        
        if (!await _fileRepository.SaveAsync(fileModel))
        {
            return Result<string>.Error("File Save error to db");
        }
        
        return Result<string>.Success(fileModel.Id,"File uploaded successfully");
    }
}