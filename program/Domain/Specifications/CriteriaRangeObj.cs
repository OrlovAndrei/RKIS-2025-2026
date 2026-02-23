namespace Domain.Specifications;

/// <summary>
/// Критерий для фильтрации по диапазону значений.
/// </summary>
/// <typeparam name="TRange">Тип диапазона, должен реализовать интерфейс IComparable&lt;TInRange&gt;</typeparam>
/// <typeparam name="TInRange">Тип значения для сравнения с диапазоном.</typeparam>
public class CriteriaRangeObj<TRange, TInRange>(TRange value) where TRange : IComparable<TInRange>
{
    /// <summary>
    /// Флаг инверсии результата сравнения.
    /// </summary>
    public bool NotFlag { get; private set; }
    
    /// <summary>
    /// Значение диапазона для сравнения.
    /// </summary>
    public readonly TRange Value = value;
    
    /// <summary>
    /// Инвертирует результат сравнения.
    /// </summary>
    /// <returns>Возвращает текущий объект для цепочки вызовов.</returns>
    public CriteriaRangeObj<TRange, TInRange> Not()
    {
        NotFlag = !NotFlag;
        return this;
    }
    
    /// <summary>
    /// Проверяет, находится ли заданное значение в диапазоне критерия.
    /// </summary>
    /// <param name="other">Значение для проверки.</param>
    /// <returns>Возвращает true, если значение находится в диапазоне; иначе false.</returns>
    public bool IsSatisfiedBy(TInRange? other)
    {
        if (other is null)
        {
            return NotFlag;
        }
        var res = Value.CompareTo(other);
        return NotFlag ? res != 0 : res == 0;
    }
}