using Presentation.Output.Interfaces;
using static System.Console;

namespace Presentation.Input.Implementation;

internal class Button(IColoredOutput coloredOutput)
{
	private readonly IColoredOutput _coloredOutput = coloredOutput;
	public bool YesOrNo(string text)
	{
		_coloredOutput.WriteText($"{text} (y/N): ");
		bool result = ReadKey().Key == ConsoleKey.Y;
		_coloredOutput.WriteEmptyLine();
		return result;
	}

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
