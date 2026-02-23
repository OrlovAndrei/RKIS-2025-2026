using Presentation.Output.Interfaces;
using Presentation.Output;

namespace Presentation.Adapters;

/// <summary>
/// Адаптер для работы только с выводом информации
/// </summary>
public class OutputAdapter : IErrorOutput
{
	private readonly IColoredOutput _coloredOutput;
	private readonly IErrorOutput _errorOutput;

	public OutputAdapter(IErrorOutput? errorOutput = null, IColoredOutput? coloredOutput = null)
	{
		_errorOutput = errorOutput ?? new ConsoleOutput();
		_coloredOutput = coloredOutput ?? _errorOutput as IColoredOutput ?? new ConsoleOutput();
	}

	#region IOutputProvider
	public void WriteText(string message) => _coloredOutput.WriteText(message);

	public void WriteLine(string message) => _coloredOutput.WriteLine(message);

	public void WriteEmptyLine() => _coloredOutput.WriteEmptyLine();
	#endregion

	#region IColoredOutput
	public void WriteColoredMessage(string message, ConsoleColor color) => _coloredOutput.WriteColoredMessage(message, color);

	public void WriteColoredLine(string message, ConsoleColor color) => _coloredOutput.WriteColoredLine(message, color);

	public void WriteSuccess(string message) => _coloredOutput.WriteSuccess(message);

	public void WriteWarning(string message) => _coloredOutput.WriteWarning(message);

	public void WriteInfo(string message) => _coloredOutput.WriteInfo(message);
	#endregion

	#region IErrorOutput
	public void WriteError(string message) => _errorOutput.WriteError(message);

	public void WriteException(Exception exception) => _errorOutput.WriteException(exception);

	public void WriteDetailedException(Exception exception) => _errorOutput.WriteDetailedException(exception);

	public void WriteErrorLines(params string[] messages) => _errorOutput.WriteErrorLines(messages);
	#endregion

	/// <summary>
	/// Вывести таблицу из данных
	/// </summary>
	public void WriteTable<T>(IEnumerable<T> items, Func<T, string> format)
	{
		foreach (var item in items)
		{
			WriteLine(format(item));
		}
	}

	/// <summary>
	/// Вывести список с нумерацией
	/// </summary>
	public void WriteList<T>(IEnumerable<T> items, Func<T, string> format)
	{
		int index = 1;
		foreach (var item in items)
		{
			WriteLine($"{index}. {format(item)}");
			index++;
		}
	}

	/// <summary>
	/// Вывести меню
	/// </summary>
	public void WriteMenu(string title, params (string key, string description)[] options)
	{
		WriteInfo(title);
		WriteEmptyLine();
		foreach (var (key, description) in options)
		{
			WriteLine($"[{key}] - {description}");
		}
		WriteEmptyLine();
	}
}
