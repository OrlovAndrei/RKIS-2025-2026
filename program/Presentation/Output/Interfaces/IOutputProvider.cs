namespace Presentation.Output.Interfaces;

/// <summary>
/// Базовый интерфейс для провайдера вывода информации
/// </summary>
public interface IOutputProvider
{
	/// <summary>
	/// Вывести простой текст
	/// </summary>
	void WriteText(string message);
	
	/// <summary>
	/// Вывести текст с новой строки
	/// </summary>
	void WriteLine(string message);
	
	/// <summary>
	/// Вывести пустую строку
	/// </summary>
	void WriteEmptyLine();
}
