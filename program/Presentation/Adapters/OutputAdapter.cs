using Presentation.Output.Interfaces;
using Presentation.Output;

namespace Presentation.Adapters;

/// <summary>
/// Адаптер для работы только с выводом информации.
/// Реализует интерфейс вывода ошибок и цветного вывода.
/// </summary>
public class OutputAdapter : IErrorOutput
{
	/// <summary>
	/// Интерфейс цветного вывода.
	/// </summary>
	private readonly IColoredOutput _coloredOutput;
	/// <summary>
	/// Интерфейс вывода ошибок.
	/// </summary>
	private readonly IErrorOutput _errorOutput;

	/// <summary>
	/// Конструктор адаптера вывода.
	/// </summary>
	/// <param name="errorOutput">Интерфейс вывода ошибок (опционально).</param>
	/// <param name="coloredOutput">Интерфейс цветного вывода (опционально).</param>
	public OutputAdapter(IErrorOutput? errorOutput = null, IColoredOutput? coloredOutput = null)
	{
		_errorOutput = errorOutput ?? new ConsoleOutput();
		_coloredOutput = coloredOutput ?? _errorOutput as IColoredOutput ?? new ConsoleOutput();
	}

		#region IOutputProvider
		/// <summary>
		/// Вывести текст без переноса строки.
		/// </summary>
		public void WriteText(string message) => _coloredOutput.WriteText(message);

		/// <summary>
		/// Вывести текст с переносом строки.
		/// </summary>
		public void WriteLine(string message) => _coloredOutput.WriteLine(message);

		/// <summary>
		/// Вывести пустую строку.
		/// </summary>
		public void WriteEmptyLine() => _coloredOutput.WriteEmptyLine();
		#endregion

		#region IColoredOutput
		/// <summary>
		/// Вывести цветное сообщение без переноса строки.
		/// </summary>
		public void WriteColoredMessage(string message, ConsoleColor color) => _coloredOutput.WriteColoredMessage(message, color);

		/// <summary>
		/// Вывести цветное сообщение с переносом строки.
		/// </summary>
		public void WriteColoredLine(string message, ConsoleColor color) => _coloredOutput.WriteColoredLine(message, color);

		/// <summary>
		/// Вывести сообщение об успешном выполнении.
		/// </summary>
		public void WriteSuccess(string message) => _coloredOutput.WriteSuccess(message);

		/// <summary>
		/// Вывести предупреждение.
		/// </summary>
		public void WriteWarning(string message) => _coloredOutput.WriteWarning(message);

		/// <summary>
		/// Вывести информационное сообщение.
		/// </summary>
		public void WriteInfo(string message) => _coloredOutput.WriteInfo(message);
		#endregion

		#region IErrorOutput
		/// <summary>
		/// Вывести сообщение об ошибке.
		/// </summary>
		public void WriteError(string message) => _errorOutput.WriteError(message);

		/// <summary>
		/// Вывести исключение.
		/// </summary>
		public void WriteException(Exception exception) => _errorOutput.WriteException(exception);

		/// <summary>
		/// Вывести подробное исключение.
		/// </summary>
		public void WriteDetailedException(Exception exception) => _errorOutput.WriteDetailedException(exception);

		/// <summary>
		/// Вывести несколько сообщений об ошибке.
		/// </summary>
		public void WriteErrorLines(params string[] messages) => _errorOutput.WriteErrorLines(messages);
		#endregion

		/// <summary>
		/// Вывести таблицу из данных.
		/// </summary>
		/// <typeparam name="T">Тип элемента.</typeparam>
		/// <param name="items">Коллекция элементов.</param>
		/// <param name="format">Функция форматирования элемента.</param>
		public void WriteTable<T>(IEnumerable<T> items, Func<T, string> format)
		{
			foreach (var item in items)
			{
				WriteLine(format(item));
			}
		}

		/// <summary>
		/// Вывести список с нумерацией.
		/// </summary>
		/// <typeparam name="T">Тип элемента.</typeparam>
		/// <param name="items">Коллекция элементов.</param>
		/// <param name="format">Функция форматирования элемента.</param>
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
		/// Вывести меню.
		/// </summary>
		/// <param name="title">Заголовок меню.</param>
		/// <param name="options">Опции меню (ключ и описание).</param>
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
