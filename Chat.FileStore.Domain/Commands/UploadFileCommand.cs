using Microsoft.AspNetCore.Http;

namespace Chat.FileStore.Domain.Commands;

public class UploadFileCommand
{
    public IFormFile FormFile { get; set; }
}