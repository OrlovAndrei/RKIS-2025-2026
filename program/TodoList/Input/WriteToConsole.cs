using static System.Console;

using Spectre.Console;

namespace ShevricTodo.Input;

public static class WriteToConsole
{
	public static void ColorMessage(string textError, ConsoleColor colorText = ConsoleColor.Red)
	{
		ForegroundColor = colorText;
		WriteLine(textError);
		ResetColor();
	}
	public static void ShortText(params string[] text)
	{
		foreach (string textItem in text)
		{
			ColorMessage(textItem, ConsoleColor.DarkYellow);
		}
	}
	public static void ProcExcept(Exception ex, string text)
	{
		ColorMessage($"Исключение: {ex.Message}", ConsoleColor.Red);
		ColorMessage($"Метод: {ex.TargetSite}", ConsoleColor.Red);
		ColorMessage($"Трассировка стека: {ex.StackTrace}", ConsoleColor.DarkYellow);
		if (ex.InnerException is not null)
		{
			ColorMessage($"{ex.InnerException}", ConsoleColor.Yellow);
		}
	}
	public async static Task PrintTable(string[] columns, IEnumerable<string[]> rows, string? title = null)
	{
		var table = new Table()
			.AddColumns(columns);
		if (title is not null)
		{
			table.Title(title);
		}
		foreach(var row in rows)
		{
			table.AddRow(row);
		}
		AnsiConsole.Write(table);
	}
	public async static Task PrintPanel(string? header = null, params string[] textLines)
	{
		var panel = new Panel(string.Join(@"\n", textLines));
		if (header is not null) panel.Header(header);
		AnsiConsole.Write(panel);
	}
}
