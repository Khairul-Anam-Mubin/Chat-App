using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.ORM.Interfaces;

public interface ICompoundFilter
{
    List<IFilter> Filters { get; set; }
    List<ICompoundFilter> CompoundFilters { get; set; }
    CompoundLogic Logic { get; set; }
}