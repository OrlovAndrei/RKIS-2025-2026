using static System.Console;

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
	public static void ProcExcept(Exception ex)
	{
		ColorMessage($"Исключение: {ex.Message}", ConsoleColor.Red);
		ColorMessage($"Метод: {ex.TargetSite}", ConsoleColor.Red);
		ColorMessage($"Трассировка стека: {ex.StackTrace}", ConsoleColor.DarkYellow);
		if (ex.InnerException is not null)
		{
			ColorMessage($"{ex.InnerException}", ConsoleColor.Yellow);
		}
	}
}
