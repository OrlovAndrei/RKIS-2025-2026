using static System.Console;
using System.Text;
using Presentation.Output.Interfaces;

namespace Presentation.Input.Implementation;

internal class Text(IColoredOutput coloredOutput)
{
	private readonly IColoredOutput _coloredOutput = coloredOutput;
	public string LongText(string text)
	{
		const string endLine = @"\end";
		const string inputChar = "> ";
		List<string> stringOutList = [];
		_coloredOutput.WriteEmptyLine();
		_coloredOutput.WriteText(text);
		_coloredOutput.WriteColoredLine($"Введите '{endLine}', для окончания ввода", ConsoleColor.Green);
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
	public string ShortText(string text, bool notNull)
	{
		StringBuilder input = new();
		while (true)
		{
			_coloredOutput.WriteText(text);
			input.Append((ReadLine() ?? string.Empty).Trim());
			if (notNull)
			{
				if (!string.IsNullOrEmpty(input.ToString()))
				{
					return input.ToString();
				}
				_coloredOutput.WriteColoredLine("Строка не должна быть пустой", ConsoleColor.Red);
			}
			else
			{
				return input.ToString();
			}
		}
	}
	public string ShortText(string text) => ShortText(text, true);
}
