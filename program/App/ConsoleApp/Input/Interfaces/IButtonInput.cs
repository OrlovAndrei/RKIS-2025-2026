namespace ConsoleApp.Input.Interfaces;

/// <summary>
/// Интерфейс для операций ввода через кнопки и выбор вариантов
/// </summary>
public interface IButtonInput
{
	/// <summary>
	/// Получить ответ да/нет от пользователя
	/// </summary>
	bool GetYesNoChoice(string prompt);
	
	/// <summary>
	/// Получить один вариант из списка
	/// </summary>
	string GetSelectionFromList(IEnumerable<string> options, string? title = null, int pageSize = 3);
	
	/// <summary>
	/// Получить один вариант из словаря
	/// </summary>
	KeyValuePair<int, string> GetSelectionFromDictionary(Dictionary<int, string> options, string? title = null, int pageSize = 3);
	
	/// <summary>
	/// Получить нажатую кнопку из набора
	/// </summary>
	ConsoleKey GetKeyFromSet(string prompt, ConsoleKey defaultKey = ConsoleKey.Y, params ConsoleKey[] allowedKeys);
}
