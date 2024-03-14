using Chat.FileStore.Domain.Entities;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.FileStore.Domain.Repositories;

public interface IFileRepository : IRepository<FileModel>
{

}