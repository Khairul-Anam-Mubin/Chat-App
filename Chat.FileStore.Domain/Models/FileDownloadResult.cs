namespace Chat.FileStore.Domain.Models;

public class FileDownloadResult
{
    public FileModel FileModel { get; set; }
    public string ContentType { get; set; }
    public byte[] FileBytes { get; set; }
}