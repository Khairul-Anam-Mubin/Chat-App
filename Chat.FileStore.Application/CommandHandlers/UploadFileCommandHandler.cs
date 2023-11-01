using Chat.Application.Shared.Providers;
using Chat.FileStore.Application.Interfaces;
using Chat.FileStore.Domain.Commands;
using Chat.FileStore.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<UploadFileCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, CommandResponse>
{
    private readonly IFileRepository _fileRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UploadFileCommandHandler(IFileRepository fileRepository, IHttpContextAccessor httpContextAccessor)
    {
        _fileRepository = fileRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommandResponse> HandleAsync(UploadFileCommand command)
    {
        var response = command.CreateResponse();

        var file = command.FormFile;
        var pathToSave = "C:\\workstation\\Training\\Chat-WebApp\\Chat.FileStore.Persistence\\Store";
        
        if (file.Length <= 0)
        {
            throw new Exception("File Length 0");
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
            throw new Exception("File Save error to db");
        }
        
        response.Message = "File uploaded successfully";
        response.SetData("FileId", fileModel.Id);
        
        return response;
    }
}