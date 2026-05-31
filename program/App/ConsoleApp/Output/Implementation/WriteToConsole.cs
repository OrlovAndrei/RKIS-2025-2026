using static System.Console;

using Spectre.Console;

namespace ConsoleApp.Output.Implementation;

public static class WriteToConsole
{
	/// <summary>
	/// Writes the specified message to the console using the given text color.
	/// </summary>
	/// <remarks>This method temporarily changes the console's foreground color to the specified value while writing
	/// the message, and then restores the original color. Use this method to highlight error messages or other important
	/// information in the console output.</remarks>
	/// <param name="textError">The message to display in the console output.</param>
	/// <param name="colorText">The color to use for the message text. Defaults to <see cref="ConsoleColor.Red"/> if not specified.</param>
	public static void ColorMessage(string textError, ConsoleColor colorText = ConsoleColor.Red)
	{
		ForegroundColor = colorText;
		WriteLine(textError);
		ResetColor();
	}
	/// <summary>
	/// Displays each provided text string in a dark yellow color on the console.
	/// </summary>
	/// <remarks>This method is useful for highlighting messages in the console output, making them stand out
	/// visually.</remarks>
	/// <param name="text">The variable-length array of strings to be displayed. Each string is printed on a new line.</param>
	public static void LongText(params string[] text)
	{
		foreach (string textItem in text)
		{
			ColorMessage(textItem, ConsoleColor.DarkYellow);
		}
	}
	/// <summary>
	/// Processes and displays detailed information about an exception to the console, including the exception message, the
	/// method where the exception occurred, the stack trace, and any inner exception details.
	/// </summary>
	/// <remarks>This method is useful for logging or debugging purposes, as it highlights critical exception
	/// details in different console colors for improved readability.</remarks>
	/// <param name="ex">The exception to process and display. Cannot be null.</param>
	public static void ProcExcept(Exception ex)
	{
		ColorMessage($"Исключение: {ex.Message}", ConsoleColor.Red);
		ColorMessage($"Метод: {ex.TargetSite}", ConsoleColor.Red);
		ColorMessage($"Трассировка стека:", ConsoleColor.Yellow);
		ColorMessage($"{ex.StackTrace}", ConsoleColor.DarkYellow);
		if (ex.InnerException is not null)
		{
			ColorMessage($"{ex.InnerException}", ConsoleColor.Yellow);
		}
	}
	/// <summary>
	/// Asynchronously displays a formatted table in the console with the specified columns and rows, and optionally
	/// includes a title above the table.
	/// </summary>
	/// <remarks>Await this method to ensure the table is fully rendered before executing subsequent operations. The
	/// method does not return any data and is intended for output purposes only.</remarks>
	/// <param name="columns">An array of strings that defines the names of the columns to display in the table. Each element represents a column
	/// header.</param>
	/// <param name="rows">A collection of string arrays, where each array contains the values for a single row in the table. The number of
	/// elements in each row should match the number of columns.</param>
	/// <param name="title">An optional string specifying the title to display above the table. If null, no title is shown.</param>
	/// <returns>A task that represents the asynchronous operation of printing the table to the console.</returns>
	public static void PrintTable(string[] columns, IEnumerable<string[]> rows, string? title = null)
	{
		var table = new Table()
			.RoundedBorder()
			.BorderColor(Color.Blue)
			.AddColumns(columns);
		if (title is not null)
		{
			table.Title(title);
		}
		foreach (string[] row in rows)
		{
			table.AddRow(row);
		}
		AnsiConsole.Write(table);
	}
	/// <summary>
	/// Renders a panel to the console with optional header text and one or more lines of content.
	/// </summary>
	/// <remarks>The method uses AnsiConsole to output the panel. All provided text lines are joined with newline
	/// characters to form the panel's content.</remarks>
	/// <param name="header">The optional header text to display at the top of the panel. If null, the panel is rendered without a header.</param>
	/// <param name="textLines">The lines of text to display within the panel. Each element represents a separate line.</param>
	/// <returns>A task that represents the asynchronous operation of rendering the panel.</returns>
	public static void PrintPanel(string? header = null, params string[] textLines)
	{
		var panel = new Panel(string.Join(Environment.NewLine, textLines))
			.RoundedBorder()
			.BorderColor(Color.Blue);
		if (header is not null)
		{
			panel.Header(header);
		}
		AnsiConsole.Write(panel);
	}
}
