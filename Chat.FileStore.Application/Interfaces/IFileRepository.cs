using Chat.FileStore.Domain.Models;
using Chat.Framework.Database.Interfaces;

namespace Chat.FileStore.Application.Interfaces;

public interface IFileRepository : IRepository<FileModel>
{

}