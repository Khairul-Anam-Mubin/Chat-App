using Microsoft.AspNetCore.Http;

namespace Chat.FileStore.Application.Commands;

public class UploadFileCommand
{
    public IFormFile FormFile { get; set; }
}