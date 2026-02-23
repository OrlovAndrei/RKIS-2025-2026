namespace Domain.Specifications;

/// <summary>
/// Критерий для фильтрации по значению объекта с поддержкой различных типов сравнения.
/// </summary>
/// <typeparam name="T">Тип значения для сравнения, должен реализовать интерфейс IComparable&lt;T&gt;</typeparam>
public class CriteriaObj<T>(T value) where T : IComparable<T>
{
    /// <summary>
    /// Тип сравнения для проверки критерия.
    /// </summary>
    public enum CompareType
    {
        /// <summary>Точное совпадение.</summary>
        Equals,
        /// <summary>Содержит подстроку.</summary>
        Contains,
        /// <summary>Начинается с подстроки.</summary>
        StartsWith,
        /// <summary>Заканчивается подстрокой.</summary>
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
    /// Проверяет, удовлетворяет ли заданное значение критерию с учетом типа сравнения.
    /// </summary>
    /// <param name="other">Значение для проверки.</param>
    /// <returns>Возвращает true, если значение удовлетворяет критерию; иначе false.</returns>
    public bool IsSatisfiedBy(T? other)
    {
        if (other is null)
        {
            return NotFlag;
        }
        var res = _compareType switch
        {
            CompareType.Equals => other.CompareTo(Value),
            CompareType.Contains => other.ToString()?.Contains(Value.ToString() ?? "") == true ? 0 : -1,
            CompareType.StartsWith => other.ToString()?.StartsWith(Value.ToString() ?? "") == true ? 0 : -1,
            CompareType.EndWith => other.ToString()?.EndsWith(Value.ToString() ?? "") == true ? 0 : -1,
            _ => throw new Exception()
        };
        return NotFlag ? res != 0 : res == 0;
    }
}
