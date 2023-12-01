using System.Linq.Expressions;
namespace Chat.Framework;

public class ExpressionHelper
{
    public static string GetFieldKey<TEntity, TField>(Expression<Func<TEntity, TField>> field)
    {
        var expression = (MemberExpression)field.Body;

        var exactExpression = expression.ToString();

        var index = exactExpression.IndexOf('.');

        return exactExpression.Substring(index + 1);
    }
}