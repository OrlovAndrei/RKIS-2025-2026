using static System.Console;
using ConsoleApp.Output.Interfaces;

namespace ConsoleApp.Output;

/// <summary>
/// Реализация цветного вывода информации в консоль
/// </summary>
public class ConsoleOutput : IErrorOutput
{
	public void WriteText(string message) => Write(message);

	public void WriteLine(string message) => Console.WriteLine(message);

	public void WriteEmptyLine() => Console.WriteLine();

	public void WriteColoredMessage(string message, ConsoleColor color)
	{
		ForegroundColor = color;
		Write(message);
		ResetColor();
	}

	public void WriteColoredLine(string message, ConsoleColor color)
	{
		ForegroundColor = color;
		Console.WriteLine(message);
		ResetColor();
	}

	public void WriteSuccess(string message) => WriteColoredLine(message, ConsoleColor.Green);

	public void WriteWarning(string message) => WriteColoredLine(message, ConsoleColor.Yellow);

	public void WriteInfo(string message) => WriteColoredLine(message, ConsoleColor.Cyan);

	public void WriteError(string message) => WriteColoredLine(message, ConsoleColor.Red);

	public void WriteException(Exception exception) => WriteColoredLine($"Исключение: {exception.Message}", ConsoleColor.Red);

	public void WriteDetailedException(Exception exception)
	{
		WriteColoredLine($"Исключение: {exception.Message}", ConsoleColor.Red);
		WriteColoredLine($"Метод: {exception.TargetSite}", ConsoleColor.Red);
		WriteColoredLine("Трассировка стека:", ConsoleColor.Yellow);
		WriteColoredLine($"{exception.StackTrace}", ConsoleColor.DarkYellow);
		
		if (exception.InnerException is not null)
		{
			WriteColoredLine("Внутреннее исключение:", ConsoleColor.Magenta);
			WriteDetailedException(exception.InnerException);
		}
	}

	public void WriteErrorLines(params string[] messages)
	{
		foreach (string message in messages)
		{
			WriteError(message);
		}
	}
}
