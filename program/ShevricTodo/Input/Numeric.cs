using static ShevricTodo.Input.Text;
using static ShevricTodo.Input.WriteToConsole;

namespace ShevricTodo.Input;

internal static class Numeric
{
	/// <summary>
	/// Ввод целого числа с границами допустимых значений
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <param name="min">Минимум</param>
	/// <param name="max">Максимум</param>
	/// <returns>Целочисленное значение соответствующие заданным границам</returns>
	public static int NumericWithMinMax(string text, int min, int max)
	{
		int input;
		while (true)
		{
			input = OneNumeric(text);
			if (input >= min && input <= max)
			{
				return input;
			}
			ColorMessage($"быть меньше или равно (<=) {min},", ConsoleColor.Red);
			ColorMessage($"быть больше или равно (>=) {max}.", ConsoleColor.Red);
		}
	}
	public static int NumericWithMin(string text, int min)
	{
		int input;
		while (true)
		{
			input = OneNumeric(text);
			if (input >= min)
			{
				return input;
			}
			ColorMessage($"быть меньше или равно (<=) {min},", ConsoleColor.Red);
		}
	}
	public static int NumericWithMax(string text, int max)
	{
		int input;
		while (true)
		{
			input = OneNumeric(text);
			if (input <= max)
			{
				return input;
			}
			ColorMessage($"быть больше или равно (>=) {max}.", ConsoleColor.Red);
		}
	}
	public static int OneNumeric(string text)
	{
		int result;
		while (true)
		{
			string input = ShortText(text);
			if (int.TryParse(input, out result))
			{
				return result;
			}
			ColorMessage($"'{input}' должно являться целым числом.", ConsoleColor.Red);
		}
	}
	public static int PositiveNumeric(string text) => NumericWithMin(text, min: 0);
}
