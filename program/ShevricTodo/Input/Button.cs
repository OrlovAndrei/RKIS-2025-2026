using static System.Console;

namespace ShevricTodo.Input;

internal static class Button
{
	public static bool YesOrNo(string text)
	{
		Write($"{text} (y/N): ");
		bool result = ReadKey().Key == ConsoleKey.Y;
		WriteLine();
		return result;
	}
	public static void OneOfButton(string text, out ConsoleKey result,
		ConsoleKey @default = ConsoleKey.Y, params ConsoleKey[] keys)
	{
		List<string> allKey = [];
		allKey.Add(@default.ToString().ToUpper());
		allKey.AddRange(keys.Select(k => k.ToString().ToLower()));
		Write($"{text} ({string.Join("/", allKey)}): ");
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
		WriteLine();
	}
}
