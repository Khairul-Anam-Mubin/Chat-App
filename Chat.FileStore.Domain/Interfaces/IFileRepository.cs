using Chat.FileStore.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.FileStore.Domain.Interfaces;

public interface IFileRepository : IRepository<FileModel>
{

}