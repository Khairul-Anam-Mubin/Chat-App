using Chat.FileStore.Domain.Models;

namespace Chat.FileStore.Application.DTOs;

public class FileDownloadResult
{
    public FileModel FileModel { get; set; }
    public string ContentType { get; set; }
    public byte[] FileBytes { get; set; }
}