using static System.Console;
using static ShevricTodo.Input.WriteToConsole;
using System.Text;

namespace ShevricTodo.Input;

internal class Text
{
	public static string LongText(string text)
	{
		string endLine = @"\end";
		string inputChar = "> ";
		List<string> stringOutList = new();
		WriteLine(text);
		ColorMessage($"Введите '{endLine}', для окончания ввода", ConsoleColor.Green);
		while (true)
		{
			string input = ShortText(inputChar, false);
			if (input != endLine)
			{
				stringOutList.Add(input);
			}
			else { break; }
		}
		return string.Join(" ", stringOutList.ToArray()); ;
	}
	/// <summary>
	/// Однострочный ввод строки
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <param name="notNull">Не допускается ли null(при значении false позволяет 
	/// ввести пустую строку)</param>
	/// <returns>Строка готовая к использованию</returns>
	public static string ShortText(string text, bool notNull = true)
	{
		StringBuilder input = new();
		while (true)
		{
			Write(text);
			input.Append((ReadLine() ?? string.Empty).Trim());
			if (notNull)
			{
				if (!string.IsNullOrEmpty(input.ToString()))
				{
					return input.ToString();
				}
				ColorMessage("Строка не должна быть пустой", ConsoleColor.Red);
			}
			else
			{
				return input.ToString();
			}
		}
	}
}
