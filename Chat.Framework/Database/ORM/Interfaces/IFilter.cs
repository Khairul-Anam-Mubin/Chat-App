using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.ORM.Interfaces;

public interface IFilter
{
    List<ISimpleFilter> SimpleFilters { get; set; }
    List<IFilter> CompoundFilters { get; set; }
    CompoundLogic? Logic { get; set; }
}