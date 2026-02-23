namespace Domain.Specifications;

/// <summary>
/// Критерий для фильтрации по точному совпадению значения.
/// </summary>
/// <typeparam name="T">Тип значения для сравнения, должен реализовать интерфейс IComparable&lt;T&gt;</typeparam>
public class CriteriaId<T>(T value) where T : IComparable<T>
{
    /// <summary>
    /// Флаг инверсии результата сравнения.
    /// </summary>
    public bool NotFlag { get; private set; }
    
    /// <summary>
    /// Значение для сравнения.
    /// </summary>
    public readonly T Value = value;
    
    /// <summary>
    /// Инвертирует результат сравнения.
    /// </summary>
    /// <returns>Возвращает текущий объект для цепочки вызовов.</returns>
    public CriteriaId<T> Not()
    {
        NotFlag = !NotFlag;
        return this;
    }
    
    /// <summary>
    /// Проверяет, удовлетворяет ли заданное значение критерию.
    /// </summary>
    /// <param name="other">Значение для проверки.</param>
    /// <returns>Возвращает true, если значение удовлетворяет критерию; иначе false.</returns>
    public bool IsSatisfiedBy(T other)
    {
        var res = Value.CompareTo(other) == 0;
        return NotFlag ? !res : res;
    }
}