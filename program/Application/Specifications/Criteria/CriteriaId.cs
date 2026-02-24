namespace Application.Specifications.Criteria;

using System.Linq.Expressions;

/// <summary>
/// Критерий для фильтрации по точному совпадению значения.
/// Строит выражение `Expression<Func<TEntity, bool>>` по селектору свойства.
/// </summary>
/// <typeparam name="T">Тип значения для сравнения, должен реализовать интерфейс IComparable&lt;T&gt;</typeparam>
public class CriteriaId<T>(T value) where T : IComparable<T>
{
    public bool NotFlag { get; private set; }

    public readonly T Value = value;

    public CriteriaId<T> Not()
    {
        NotFlag = !NotFlag;
        return this;
    }

    /// <summary>
    /// Строит выражение для фильтрации сущности по селектору свойства.
    /// </summary>
    public Expression<Func<TEntity, bool>> IsSatisfiedBy<TEntity>(Expression<Func<TEntity, T>> selector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var param = selector.Parameters[0];
        Expression member = selector.Body;
        if (member is UnaryExpression ue && ue.NodeType == ExpressionType.Convert)
            member = ue.Operand;

        var valueConst = Expression.Constant(Value, typeof(T));
        var compareTo = Expression.Call(valueConst, typeof(T).GetMethod("CompareTo", [typeof(T)])!, member);
        var compareExpr = Expression.Equal(compareTo, Expression.Constant(0));

        // null handling for reference/nullable types
        Expression body;
        var memberType = member.Type;
        bool canBeNull = !memberType.IsValueType || Nullable.GetUnderlyingType(memberType) != null;
        if (canBeNull)
        {
            var nullCheck = Expression.Equal(member, Expression.Constant(null, memberType));
            var resultIfNull = Expression.Constant(NotFlag);
            Expression resultIfNotNull = NotFlag ? Expression.Not(compareExpr) : compareExpr;
			body = Expression.Condition(nullCheck, resultIfNull, resultIfNotNull);
        }
        else
        {
            body = NotFlag ? Expression.Not(compareExpr) : compareExpr;
		}

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
}