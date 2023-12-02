using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Filters;

public class Filter : IFilter
{
    public List<ISimpleFilter> SimpleFilters { get; set; }
    public List<IFilter> CompoundFilters { get; set; }
    public CompoundLogic? Logic { get; set; }

    public Filter()
    {
        SimpleFilters = new List<ISimpleFilter>();
        CompoundFilters = new List<IFilter>();
    }

    public Filter(CompoundLogic logic, ISimpleFilter simpleFilter, params ISimpleFilter[] filters)
    {
        SimpleFilters = filters.ToList();
        SimpleFilters.Add(simpleFilter);
        CompoundFilters = new List<IFilter>();
    }

    public Filter(CompoundLogic logic, IFilter filter, params IFilter[] filters)
    {
        CompoundFilters = filters.ToList();
        CompoundFilters.Add(filter);
        SimpleFilters = new List<ISimpleFilter>();
    }

    public Filter(CompoundLogic logic, List<ISimpleFilter> filters)
    {
        SimpleFilters = filters;
        CompoundFilters = new List<IFilter>();
        Logic = logic;
    }

    public Filter(CompoundLogic logic, List<IFilter> compoundFilters)
    {
        SimpleFilters = new List<ISimpleFilter>();
        CompoundFilters = compoundFilters;
        Logic = logic;
    }

    public Filter(CompoundLogic logic, IFilter filter, List<ISimpleFilter> filters)
    {
        Logic = logic;
        SimpleFilters = filters;
        CompoundFilters = new List<IFilter> { filter };
    }
}