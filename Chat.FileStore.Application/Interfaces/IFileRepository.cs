using Chat.FileStore.Domain.Models;

namespace Chat.FileStore.Application.Interfaces;

public interface IFileRepository
{
    Task<bool> SaveFileModelAsync(FileModel fileModel);
    Task<FileModel?> GetFileModelByIdAsync(string id);
}