namespace ConsoleApp.Output.Interfaces;

/// <summary>
/// Интерфейс для вывода ошибок и исключений
/// </summary>
public interface IErrorOutput : IColoredOutput
{
	/// <summary>
	/// Вывести сообщение об ошибке
	/// </summary>
	void WriteError(string message);
	
	/// <summary>
	/// Вывести информацию об исключении
	/// </summary>
	void WriteException(Exception exception);
	
	/// <summary>
	/// Вывести детальную информацию об исключении с трассировкой стека
	/// </summary>
	void WriteDetailedException(Exception exception);
	
	/// <summary>
	/// Вывести несколько строк текста с логированием ошибок
	/// </summary>
	void WriteErrorLines(params string[] messages);
}
