using Chat.Framework.ORM.Enums;

namespace Chat.Framework.ORM.Interfaces;

public interface IDbContextFactory
{
    IDbContext GetDbContext(Context context);
}