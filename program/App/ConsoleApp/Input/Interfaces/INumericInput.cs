namespace ConsoleApp.Input.Interfaces;

/// <summary>
/// Интерфейс для операций ввода числовых значений
/// </summary>
public interface INumericInput
{
	/// <summary>
	/// Получить целое число без ограничений
	/// </summary>
	int GetNumeric(string prompt);
	
	/// <summary>
	/// Получить целое число в заданном диапазоне
	/// </summary>
	int GetNumericInRange(string prompt, int min, int max);
	
	/// <summary>
	/// Получить целое число больше или равно минимуму
	/// </summary>
	int GetNumericWithMin(string prompt, int min);
	
	/// <summary>
	/// Получить целое число меньше или равно максимуму
	/// </summary>
	int GetNumericWithMax(string prompt, int max);
	
	/// <summary>
	/// Получить положительное целое число (>= 0)
	/// </summary>
	int GetPositiveNumeric(string prompt);
}
