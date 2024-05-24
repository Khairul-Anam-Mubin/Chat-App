using System.ComponentModel.DataAnnotations;
using Peacious.Framework.CQRS;
using Microsoft.AspNetCore.Http;

namespace Chat.FileStore.Application.Commands;

public class UploadFileCommand : ICommand
{
    [Required]
    public IFormFile FormFile { get; set; }
}