namespace Domain.Specifications;

/// <summary>
/// Представляет диапазон значений с от границей и до границей.
/// </summary>
/// <typeparam name="T">Тип значений диапазона. Должен быть структурой, реализующей IComparable&lt;T&gt;</typeparam>
public class RangeObj<T> : IComparable<T> where T : struct, IComparable<T>
{
    /// <summary>
    /// Начало диапазона (минимальное значение). Null означает отсутствие нижней границы.
    /// </summary>
    public T? From { get; }
    
    /// <summary>
    /// Конец диапазона (максимальное значение). Null означает отсутствие верхней границы.
    /// </summary>
    public T? To { get; }
    /// <summary>
    /// Инициализирует новый экземпляр класса RangeObj.
    /// </summary>
    /// <param name="from">Начало диапазона.</param>
    /// <param name="to">Конец диапазона.</param>
    /// <exception cref="Exception">Выбрасывается, если from больше to.</exception>
    public RangeObj(
        T? from,
        T? to
    )
    {
        if (from.HasValue &&
            to.HasValue &&
            from.Value.CompareTo(to.Value) < 0)
        {
            throw new Exception();
        }
        From = from;
        To = to;
    }
    /// <summary>
    /// Проверяет, находится ли значение в пределах диапазона.
    /// </summary>
    /// <param name="value">Значение для проверки.</param>
    /// <returns>Возвращает true, если значение находится в диапазоне; иначе false.</returns>
    public bool Contains(T value)
    {
        if (From.HasValue && value.CompareTo(From.Value) < 0) return false;
        if (To.HasValue && value.CompareTo(To.Value) > 0) return false;
        return true;
    }

	/// <summary>
    /// Сравнивает значение с диапазоном.
    /// </summary>
    /// <param name="other">Значение для сравнения.</param>
    /// <returns>Возвращает 0, если значение находится в диапазоне; отрицательное значение, если оно меньше от границы; положительное значение, если оно больше до границы.</returns>
    public int CompareTo(T other)
	{
		const int res = 0;
        int resFrom;
        int resTo;
		if (From.HasValue && (resFrom = other.CompareTo(From.Value)) < 0) return resFrom;
		else if (To.HasValue && (resTo = other.CompareTo(To.Value)) > 0) return resTo;
		return res;
	}
}