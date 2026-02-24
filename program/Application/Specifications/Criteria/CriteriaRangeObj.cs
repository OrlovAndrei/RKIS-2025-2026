namespace Application.Specifications.Criteria;

using System.Linq.Expressions;

/// <summary>
/// Критерий для фильтрации по диапазону значений.
/// Строит выражение `Expression<Func<TEntity, bool>>` по селектору свойства.
/// </summary>
/// <typeparam name="TRange">Тип диапазона, должен реализовать интерфейс IComparable&lt;TInRange&gt;</typeparam>
/// <typeparam name="TInRange">Тип значения для сравнения с диапазоном.</typeparam>
public class CriteriaRangeObj<TRange, TInRange>(TRange value) where TRange : IComparable<TInRange>
{
    public bool NotFlag { get; private set; }

    public readonly TRange Value = value;

    public CriteriaRangeObj<TRange, TInRange> Not()
    {
        NotFlag = !NotFlag;
        return this;
    }

    /// <summary>
    /// Строит выражение для фильтрации сущности по селектору свойства.
    /// </summary>
    public Expression<Func<TEntity, bool>> IsSatisfiedBy<TEntity>(Expression<Func<TEntity, TInRange>> selector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var param = selector.Parameters[0];
        Expression member = selector.Body;
        if (member is UnaryExpression ue && ue.NodeType == ExpressionType.Convert)
            member = ue.Operand;

        // Value.CompareTo(member) == 0
        var memberExpr = member;
        var valueConst = Expression.Constant(Value, typeof(TRange));
        var compareTo = Expression.Call(valueConst, typeof(TRange).GetMethod("CompareTo", [typeof(TInRange)])!, memberExpr);
        var compareExpr = Expression.Equal(compareTo, Expression.Constant(0));

        // null handling for reference/nullable types
        Expression body;
        var memberType = memberExpr.Type;
        bool canBeNull = !memberType.IsValueType || Nullable.GetUnderlyingType(memberType) != null;
        if (canBeNull)
        {
            var nullCheck = Expression.Equal(memberExpr, Expression.Constant(null, memberType));
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