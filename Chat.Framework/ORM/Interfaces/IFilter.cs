using Chat.Framework.ORM.Enums;

namespace Chat.Framework.ORM.Interfaces;

public interface IFilter
{
    List<ISimpleFilter> SimpleFilters { get; set; }
    List<IFilter> CompoundFilters { get; set; }
    CompoundLogic? Logic { get; set; }
}