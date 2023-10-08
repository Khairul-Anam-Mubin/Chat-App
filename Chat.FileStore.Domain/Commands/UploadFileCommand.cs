using Chat.Framework.CQRS;
using Microsoft.AspNetCore.Http;

namespace Chat.FileStore.Domain.Commands;

public class UploadFileCommand : ACommand
{
    public IFormFile FormFile { get; set; }
    public override void ValidateCommand()
    {
        if (FormFile == null)
        {
            throw new Exception("File is null");
        }
    }
}