using Chat.FileStore.Domain.Models;
using Chat.Framework.Database.Interfaces;

namespace Chat.FileStore.Domain.Interfaces;

public interface IFileRepository : IRepository<FileModel>
{

}