using Chat.FileStore.Domain.Entities;

namespace Chat.FileStore.Application.DTOs;

public class FileDownloadResult
{
    public FileModel FileModel { get; set; }
    public string ContentType { get; set; }
    public byte[] FileBytes { get; set; }
}