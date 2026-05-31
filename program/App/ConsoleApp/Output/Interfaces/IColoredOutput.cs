namespace ConsoleApp.Output.Interfaces;

/// <summary>
/// Интерфейс для цветного вывода информации
/// </summary>
public interface IColoredOutput : IOutputProvider
{
	/// <summary>
	/// Вывести текст с указанным цветом
	/// </summary>
	void WriteColoredMessage(string message, ConsoleColor color);
	
	/// <summary>
	/// Вывести текст с указанным цветом и переводом на новую строку
	/// </summary>
	void WriteColoredLine(string message, ConsoleColor color);
	
	/// <summary>
	/// Вывести успешное сообщение (зеленый цвет по умолчанию)
	/// </summary>
	void WriteSuccess(string message);
	
	/// <summary>
	/// Вывести предупреждение (желтый цвет по умолчанию)
	/// </summary>
	void WriteWarning(string message);
	
	/// <summary>
	/// Вывести информацию (голубой цвет по умолчанию)
	/// </summary>
	void WriteInfo(string message);
}
