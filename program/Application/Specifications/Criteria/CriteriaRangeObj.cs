namespace Application.Specifications.Criteria;

/// <summary>
/// Критерий для фильтрации по диапазону значений.
/// Строит выражение `Expression Func TEntity, bool ` по селектору свойства.
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
}