using System.Linq.Expressions;
using Application.Specifications.Criteria;

namespace Infrastructure.EfRepository;

public static class CriteriaToExpression
{
    /// <summary>
    /// Строит выражение для фильтрации сущности по селектору свойства.
    /// </summary>
    public static Expression<Func<TEntity, bool>> IsSatisfiedBy<TEntity, T>(this CriteriaObj<T> criteriaObj, Expression<Func<TEntity, T>> selector) where T : IComparable<T>
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var param = selector.Parameters[0];
        Expression member = selector.Body;
        if (member is UnaryExpression ue && ue.NodeType == ExpressionType.Convert)
            member = ue.Operand;

        // подготовка константы значения
        var valueConst = Expression.Constant(criteriaObj.Value, typeof(T));

        Expression compareExpr;

        if (criteriaObj.CompareType == SearchTypes.Equals)
        {
            var compareTo = Expression.Call(member, typeof(T).GetMethod("CompareTo", new[] { typeof(T) })!, valueConst);
            compareExpr = Expression.Equal(compareTo, Expression.Constant(0));
        }
        else
        {
            // привести оба к string и вызвать соответствующие методы
            var memberStr = Expression.Call(member, typeof(object).GetMethod("ToString")!);
            var valueStr = Expression.Constant(criteriaObj.Value?.ToString() ?? string.Empty, typeof(string));
            var methodName = criteriaObj.CompareType switch
            {
                SearchTypes.Contains => "Contains",
                SearchTypes.StartsWith => "StartsWith",
                SearchTypes.EndsWith => "EndsWith",
                _ => throw new InvalidOperationException()
            };
            compareExpr = Expression.Call(memberStr, typeof(string).GetMethod(methodName, [typeof(string)])!, valueStr);
        }

        // null-проверка для ссылочных типов и Nullable<T>
        Expression body;
        var memberType = member.Type;
        bool canBeNull = !memberType.IsValueType || Nullable.GetUnderlyingType(memberType) != null;
        if (canBeNull)
        {
            var nullCheck = Expression.Equal(member, Expression.Constant(null, memberType));
            var resultIfNull = Expression.Constant(criteriaObj.NotFlag);
            Expression resultIfNotNull = criteriaObj.NotFlag ? Expression.Not(compareExpr) : compareExpr;
			body = Expression.Condition(nullCheck, resultIfNull, resultIfNotNull);
        }
        else
        {
            body = criteriaObj.NotFlag ? Expression.Not(compareExpr) : compareExpr;
		}

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
    /// <summary>
    /// Строит выражение для фильтрации сущности по селектору свойства.
    /// </summary>
    public static Expression<Func<TEntity, bool>> IsSatisfiedBy<TEntity, TRange, TInRange>
    (this CriteriaRangeObj<TRange, TInRange> criteriaRangeObj,
    Expression<Func<TEntity, TInRange>> selector) where TRange : IComparable<TInRange>
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var param = selector.Parameters[0];
        Expression member = selector.Body;
        if (member is UnaryExpression ue && ue.NodeType == ExpressionType.Convert)
            member = ue.Operand;

        // Value.CompareTo(member) == 0
        var memberExpr = member;
        var valueConst = Expression.Constant(criteriaRangeObj.Value, typeof(TRange));
        var compareTo = Expression.Call(valueConst, typeof(TRange).GetMethod("CompareTo", [typeof(TInRange)])!, memberExpr);
        var compareExpr = Expression.Equal(compareTo, Expression.Constant(0));

        // null handling for reference/nullable types
        Expression body;
        var memberType = memberExpr.Type;
        bool canBeNull = !memberType.IsValueType || Nullable.GetUnderlyingType(memberType) != null;
        if (canBeNull)
        {
            var nullCheck = Expression.Equal(memberExpr, Expression.Constant(null, memberType));
            var resultIfNull = Expression.Constant(criteriaRangeObj.NotFlag);
            Expression resultIfNotNull = criteriaRangeObj.NotFlag ? Expression.Not(compareExpr) : compareExpr;
			body = Expression.Condition(nullCheck, resultIfNull, resultIfNotNull);
        }
        else
        {
            body = criteriaRangeObj.NotFlag ? Expression.Not(compareExpr) : compareExpr;
		}

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
    /// <summary>
    /// Строит выражение для фильтрации сущности по селектору свойства.
    /// </summary>
    public static Expression<Func<TEntity, bool>> IsSatisfiedBy<TEntity, T>(this CriteriaId<T> criteriaId, Expression<Func<TEntity, T>> selector) where T : IComparable<T>
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var param = selector.Parameters[0];
        Expression member = selector.Body;
        if (member is UnaryExpression ue && ue.NodeType == ExpressionType.Convert)
            member = ue.Operand;

        var valueConst = Expression.Constant(criteriaId.Value, typeof(T));
        var compareTo = Expression.Call(valueConst, typeof(T).GetMethod("CompareTo", [typeof(T)])!, member);
        var compareExpr = Expression.Equal(compareTo, Expression.Constant(0));

        // null handling for reference/nullable types
        Expression body;
        var memberType = member.Type;
        bool canBeNull = !memberType.IsValueType || Nullable.GetUnderlyingType(memberType) != null;
        if (canBeNull)
        {
            var nullCheck = Expression.Equal(member, Expression.Constant(null, memberType));
            var resultIfNull = Expression.Constant(criteriaId.NotFlag);
            Expression resultIfNotNull = criteriaId.NotFlag ? Expression.Not(compareExpr) : compareExpr;
			body = Expression.Condition(nullCheck, resultIfNull, resultIfNotNull);
        }
        else
        {
            body = criteriaId.NotFlag ? Expression.Not(compareExpr) : compareExpr;
		}

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
}