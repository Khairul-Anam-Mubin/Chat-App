using Chat.Application.Shared.Providers;
using Chat.FileStore.Application.Commands;
using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.Results;
using Microsoft.AspNetCore.Http;
using IResult = Chat.Framework.Results.IResult;

namespace Chat.FileStore.Application.CommandHandlers;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UploadFileCommandHandler(IFileRepository fileRepository, IHttpContextAccessor httpContextAccessor)
    {
        _fileRepository = fileRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult> HandleAsync(UploadFileCommand command)
    {
        var response = Result.Success();

        var file = command.FormFile;
        var pathToSave = "C:\\workstation\\Training\\Chat-WebApp\\Chat.FileStore.Persistence\\Store";
        
        if (file.Length <= 0)
        {
            return Result.Error("File Length 0");
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
            return Result.Error("File Save error to db");
        }
        
        response.Message = "File uploaded successfully";
        response.SetData("FileId", fileModel.Id);
        
        return response;
    }
}