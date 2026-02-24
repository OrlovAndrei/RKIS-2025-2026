namespace Application.Specifications.Criteria;

using System.Linq.Expressions;

/// <summary>
/// Критерий для фильтрации по значению объекта с поддержкой различных типов сравнения.
/// Строит выражение `Expression<Func<TEntity, bool>>` по селектору свойства.
/// </summary>
/// <typeparam name="T">Тип значения для сравнения, должен реализовать интерфейс IComparable&lt;T&gt;</typeparam>
public class CriteriaObj<T>(T value) where T : IComparable<T>
{
    public enum CompareType
    {
        Equals,
        Contains,
        StartsWith,
        EndWith
    }

    public bool NotFlag { get; private set; }
    private CompareType _compareType = CompareType.Equals;
    public readonly T Value = value;

    public CriteriaObj<T> Not()
    {
        NotFlag = !NotFlag;
        return this;
    }
    public CriteriaObj<T> Contains()
    {
        _compareType = CompareType.Contains;
        return this;
    }
    public CriteriaObj<T> StartsWith()
    {
        _compareType = CompareType.StartsWith;
        return this;
    }
    public CriteriaObj<T> EndWith()
    {
        _compareType = CompareType.EndWith;
        return this;
    }
    public CriteriaObj<T> Equals()
    {
        _compareType = CompareType.Equals;
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

        // подготовка константы значения
        var valueConst = Expression.Constant(Value, typeof(T));

        Expression compareExpr;

        if (_compareType == CompareType.Equals)
        {
            var compareTo = Expression.Call(member, typeof(T).GetMethod("CompareTo", new[] { typeof(T) })!, valueConst);
            compareExpr = Expression.Equal(compareTo, Expression.Constant(0));
        }
        else
        {
            // привести оба к string и вызвать соответствующие методы
            var memberStr = Expression.Call(member, typeof(object).GetMethod("ToString")!);
            var valueStr = Expression.Constant(Value?.ToString() ?? string.Empty, typeof(string));
            var methodName = _compareType switch
            {
                CompareType.Contains => "Contains",
                CompareType.StartsWith => "StartsWith",
                CompareType.EndWith => "EndsWith",
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
