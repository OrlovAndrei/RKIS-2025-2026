namespace Application.Specifications.Criteria;

/// <summary>
/// Критерий для фильтрации по значению объекта с поддержкой различных типов сравнения.
/// Строит выражение `Expression Func TEntity, bool ` по селектору свойства.
/// </summary>
/// <typeparam name="T">Тип значения для сравнения, должен реализовать интерфейс IComparable&lt;T&gt;</typeparam>
public class CriteriaObj<T>(T value) where T : IComparable<T>
{
    public bool NotFlag { get; private set; }

    public SearchTypes CompareType = SearchTypes.Equals;

    public readonly T Value = value;

    public CriteriaObj<T> Not()
    {
        NotFlag = !NotFlag;
        return this;
    }
    public CriteriaObj<T> Contains()
    {
        CompareType = SearchTypes.Contains;
        return this;
    }
    public CriteriaObj<T> StartsWith()
    {
        CompareType = SearchTypes.StartsWith;
        return this;
    }
    public CriteriaObj<T> EndWith()
    {
        CompareType = SearchTypes.EndsWith;
        return this;
    }
    public CriteriaObj<T> Equals()
    {
        CompareType = SearchTypes.Equals;
        return this;
    }
}
