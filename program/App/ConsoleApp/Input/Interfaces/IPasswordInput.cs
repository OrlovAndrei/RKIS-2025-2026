namespace ConsoleApp.Input.Interfaces;

/// <summary>
/// Интерфейс для операций ввода пароля
/// </summary>
public interface IPasswordInput
{
	/// <summary>
	/// Получить пароль с маскированием символов
	/// </summary>
	string GetPassword(string prompt);
	
	/// <summary>
	/// Получить подтвержденный пароль (с проверкой совпадения)
	/// </summary>
	string GetCheckedPassword();
	
	/// <summary>
	/// Проверить пароль на минимальную длину
	/// </summary>
	bool ValidatePasswordLength(string password, int minLength = 8);
}
