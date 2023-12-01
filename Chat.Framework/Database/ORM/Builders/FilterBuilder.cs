using System.Linq.Expressions;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Filters;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Builders;

public class FilterBuilder<TEntity>
{
    public ICompoundFilter Eq<TField>(Expression<Func<TEntity, TField>> field, TField value)
    {
        return Eq(ExpressionHelper.GetFieldKey(field), value!);
    }

    public ICompoundFilter Eq(string fieldKey, object value)
    {
        return new CompoundFilter(CompoundLogic.And, new SimpleFilter(fieldKey, Operator.Equal, value));
    }

    public ICompoundFilter Neq<TField>(Expression<Func<TEntity, TField>> field, TField value)
    {
        return Neq(ExpressionHelper.GetFieldKey(field), value!);
    }

    public ICompoundFilter Neq(string fieldKey, object value)
    {
        return new CompoundFilter(CompoundLogic.And, new SimpleFilter(fieldKey, Operator.NotEqual, value));
    }

    public ICompoundFilter In<TField>(Expression<Func<TEntity, TField>> field, List<TField> values)
    {
        return In(ExpressionHelper.GetFieldKey(field), values);
    }
    
    public ICompoundFilter In<TField>(string fieldKey, List<TField> values)
    {
        return new CompoundFilter(CompoundLogic.And, new SimpleFilter(fieldKey, Operator.In, values));
    }

    public ICompoundFilter And(IFilter filter1, IFilter filter2, params IFilter[] filters)
    {
        var filterList = filters.ToList();
        filterList.Add(filter1);
        filterList.Add(filter2);
        return new CompoundFilter(CompoundLogic.And, filterList);
    }

    public ICompoundFilter And(ICompoundFilter compoundFilter1, ICompoundFilter compoundFilter2, params ICompoundFilter[] compoundFilters)
    {
        var filterList = compoundFilters.ToList();
        filterList.Add(compoundFilter1);
        filterList.Add(compoundFilter2);
        return new CompoundFilter(CompoundLogic.And, filterList);
    }

    public ICompoundFilter And(ICompoundFilter compoundFilter, IFilter filter, params IFilter[] filters)
    {
        var filtersList = filters.ToList();
        filtersList.Add(filter);
        return new CompoundFilter(CompoundLogic.And, compoundFilter, filtersList);
    }

    public ICompoundFilter Or(IFilter filter1, IFilter filter2, params IFilter[] filters)
    {
        var filterList = filters.ToList();
        filterList.Add(filter1);
        filterList.Add(filter2);
        return new CompoundFilter(CompoundLogic.Or, filterList);
    }

    public ICompoundFilter Or(ICompoundFilter compoundFilter1, ICompoundFilter compoundFilter2, params ICompoundFilter[] compoundFilters)
    {
        var filterList = compoundFilters.ToList();
        filterList.Add(compoundFilter1);
        filterList.Add(compoundFilter2);
        return new CompoundFilter(CompoundLogic.Or, filterList);
    }

    public ICompoundFilter Or(ICompoundFilter compoundFilter, IFilter filter, params IFilter[] filters)
    {
        var filtersList = filters.ToList();
        filtersList.Add(filter);
        return new CompoundFilter(CompoundLogic.Or, compoundFilter, filtersList);
    }
}