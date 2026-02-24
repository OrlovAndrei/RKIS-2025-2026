namespace Application.Specifications.Criteria;

/// <summary>
/// Критерий для фильтрации по точному совпадению значения.
/// Строит выражение `Expression Func TEntity, bool ` по селектору свойства.
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
}