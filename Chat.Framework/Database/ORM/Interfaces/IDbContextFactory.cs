using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.ORM.Interfaces;

public interface IDbContextFactory
{
    IDbContext? GetDbContext(Context context);
}