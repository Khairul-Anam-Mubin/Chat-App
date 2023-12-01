using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Filters;

public class CompoundFilter : ICompoundFilter
{
    public List<IFilter> Filters { get; set; }
    public List<ICompoundFilter> CompoundFilters { get; set; }
    public CompoundLogic Logic { get; set; }

    public CompoundFilter()
    {
        Filters = new List<IFilter>();
        CompoundFilters = new List<ICompoundFilter>();
    }

    public CompoundFilter(CompoundLogic logic, IFilter filter, params IFilter[] filters)
    {
        Filters = filters.ToList();
        Filters.Add(filter);
        CompoundFilters = new List<ICompoundFilter>();
    }

    public CompoundFilter(CompoundLogic logic, ICompoundFilter filter, params ICompoundFilter[] filters)
    {
        CompoundFilters = filters.ToList();
        CompoundFilters.Add(filter);
        Filters = new List<IFilter>();
    }

    public CompoundFilter(CompoundLogic logic, List<IFilter> filters)
    {
        Filters = filters;
        CompoundFilters = new List<ICompoundFilter>();
        Logic = logic;
    }

    public CompoundFilter(CompoundLogic logic, List<ICompoundFilter> compoundFilters)
    {
        Filters = new List<IFilter>();
        CompoundFilters = compoundFilters;
        Logic = logic;
    }

    public CompoundFilter(CompoundLogic logic, ICompoundFilter compoundFilter, List<IFilter> filters)
    {
        Logic = logic;
        Filters = filters;
        CompoundFilters = new List<ICompoundFilter> { compoundFilter };
    }
}