using System.Linq.Expressions;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Filters;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Builders;

public class FilterBuilder<TEntity>
{
    public IFilter None()
    {
        return new Filter();
    }

    public IFilter Eq<TField>(Expression<Func<TEntity, TField>> field, TField value)
    {
        return Eq(ExpressionHelper.GetFieldKey(field), value!);
    }

    public IFilter Eq(string fieldKey, object value)
    {
        return new Filter(CompoundLogic.And, new SimpleFilter(fieldKey, Operator.Equal, value));
    }

    public IFilter Neq<TField>(Expression<Func<TEntity, TField>> field, TField value)
    {
        return Neq(ExpressionHelper.GetFieldKey(field), value!);
    }

    public IFilter Neq(string fieldKey, object value)
    {
        return new Filter(CompoundLogic.And, new SimpleFilter(fieldKey, Operator.NotEqual, value));
    }

    public IFilter In<TField>(Expression<Func<TEntity, TField>> field, List<TField> values)
    {
        return In(ExpressionHelper.GetFieldKey(field), values);
    }
    
    public IFilter In<TField>(string fieldKey, List<TField> values)
    {
        return new Filter(CompoundLogic.And, new SimpleFilter(fieldKey, Operator.In, values));
    }

    public IFilter And(ISimpleFilter filter1, ISimpleFilter filter2, params ISimpleFilter[] filters)
    {
        var filterList = filters.ToList();
        filterList.Add(filter1);
        filterList.Add(filter2);
        return new Filter(CompoundLogic.And, filterList);
    }

    public IFilter And(IFilter filter1, IFilter filter2, params IFilter[] compoundFilters)
    {
        var filterList = compoundFilters.ToList();
        filterList.Add(filter1);
        filterList.Add(filter2);
        return new Filter(CompoundLogic.And, filterList);
    }

    public IFilter And(IFilter filter, ISimpleFilter simpleFilter, params ISimpleFilter[] filters)
    {
        var filtersList = filters.ToList();
        filtersList.Add(simpleFilter);
        return new Filter(CompoundLogic.And, filter, filtersList);
    }

    public IFilter Or(ISimpleFilter filter1, ISimpleFilter filter2, params ISimpleFilter[] filters)
    {
        var filterList = filters.ToList();
        filterList.Add(filter1);
        filterList.Add(filter2);
        return new Filter(CompoundLogic.Or, filterList);
    }

    public IFilter Or(IFilter filter1, IFilter filter2, params IFilter[] compoundFilters)
    {
        var filterList = compoundFilters.ToList();
        filterList.Add(filter1);
        filterList.Add(filter2);
        return new Filter(CompoundLogic.Or, filterList);
    }

    public IFilter Or(IFilter filter, ISimpleFilter simpleFilter, params ISimpleFilter[] filters)
    {
        var filtersList = filters.ToList();
        filtersList.Add(simpleFilter);
        return new Filter(CompoundLogic.Or, filter, filtersList);
    }
}