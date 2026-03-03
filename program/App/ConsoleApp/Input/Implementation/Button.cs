using ConsoleApp.Output.Interfaces;
using static System.Console;

namespace ConsoleApp.Input.Implementation;

/// <summary>
/// Класс для обработки кнопочных вводов пользователя.
/// </summary>
internal class Button(IColoredOutput coloredOutput)
{
	/// <summary>
	/// Интерфейс цветного вывода.
	/// </summary>
	private readonly IColoredOutput _coloredOutput = coloredOutput;

	/// <summary>
	/// Получить ответ пользователя "Да/Нет".
	/// </summary>
	/// <param name="text">Вопрос пользователю.</param>
	/// <returns>True, если пользователь нажал 'Y'.</returns>
	public bool YesOrNo(string text)
	{
		_coloredOutput.WriteText($"{text} (y/N): ");
		bool result = ReadKey().Key == ConsoleKey.Y;
		_coloredOutput.WriteEmptyLine();
		return result;
	}

	/// <summary>
	/// Получить выбор пользователя из набора клавиш.
	/// </summary>
	/// <param name="text">Вопрос пользователю.</param>
	/// <param name="result">Выбранная клавиша.</param>
	/// <param name="default">Клавиша по умолчанию.</param>
	/// <param name="keys">Разрешённые клавиши.</param>
	public void OneOfButton(string text, out ConsoleKey result,
		ConsoleKey @default = ConsoleKey.Y, params ConsoleKey[] keys)
	{
		var allKey = new List<string>
		{
			@default.ToString().ToUpper()
		};
		allKey.AddRange(keys.Select(k => k.ToString().ToLower()));
		_coloredOutput.WriteText($"{text} ({string.Join("/", allKey)}): ");
		ConsoleKey keyInput = ReadKey().Key;
		result = @default;
		foreach (ConsoleKey keySmall in keys)
		{
			if (keyInput == keySmall)
			{
				result = keyInput;
				break;
			}
		}
		_coloredOutput.WriteEmptyLine();
	}
}
