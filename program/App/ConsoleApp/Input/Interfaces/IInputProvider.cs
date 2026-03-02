namespace ConsoleApp.Input.Interfaces;

/// <summary>
/// Базовый интерфейс для провайдера ввода информации
/// </summary>
public interface IInputProvider
{
	/// <summary>
	/// Получить строку от пользователя
	/// </summary>
	string GetText(string prompt);
	
	/// <summary>
	/// Получить целое число от пользователя
	/// </summary>
	int GetNumeric(string prompt);
	
	/// <summary>
	/// Получить пароль от пользователя
	/// </summary>
	string GetPassword(string prompt);
}
