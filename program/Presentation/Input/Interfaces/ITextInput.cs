namespace Presentation.Input.Interfaces;

/// <summary>
/// Интерфейс для операций ввода текста
/// </summary>
public interface ITextInput
{
	/// <summary>
	/// Получить однострочный текст
	/// </summary>
	string GetShortText(string prompt, bool notNull = true);
	
	/// <summary>
	/// Получить многострочный текст
	/// </summary>
	string GetLongText(string prompt);
	
	/// <summary>
	/// Получить текст с проверкой на пустоту
	/// </summary>
	string GetNonEmptyText(string prompt);
}
