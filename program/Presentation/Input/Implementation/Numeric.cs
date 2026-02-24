using Presentation.Output.Interfaces;

namespace Presentation.Input.Implementation;

internal class Numeric(IColoredOutput coloredOutput)
{
	private readonly IColoredOutput _coloredOutput = coloredOutput;
	private readonly Text _text = new(coloredOutput);
	/// <summary>
	/// Ввод целого числа с границами допустимых значений
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <param name="min">Минимум</param>
	/// <param name="max">Максимум</param>
	/// <returns>Целочисленное значение соответствующие заданным границам</returns>
	public int NumericWithMinMax(string text, int min, int max)
	{
		int input;
		while (true)
		{
			input = OneNumeric(text);
			if (input >= min && input <= max)
			{
				return input;
			}
			_coloredOutput.WriteColoredLine($"Значение должно быть больше или равно (>=) {min}", ConsoleColor.Red);
			_coloredOutput.WriteColoredLine($"и меньше или равно (<=) {max}.", ConsoleColor.Red);
		}
	}
	public int NumericWithMin(string text, int min)
	{
		int input;
		while (true)
		{
			input = OneNumeric(text);
			if (input >= min)
			{
				return input;
			}
			_coloredOutput.WriteColoredLine($"Значение должно быть больше или равно (>=) {min}.", ConsoleColor.Red);
		}
	}
	public int NumericWithMax(string text, int max)
	{
		int input;
		while (true)
		{
			input = OneNumeric(text);
			if (input <= max)
			{
				return input;
			}
			_coloredOutput.WriteColoredLine($"Значение должно быть меньше или равно (<=) {max}.", ConsoleColor.Red);
		}
	}
	public int OneNumeric(string text)
	{
		int result;
		while (true)
		{
			string input = _text.ShortText(text);
			if (int.TryParse(input, out result))
			{
				return result;
			}
			_coloredOutput.WriteColoredLine($"'{input}' должно являться целым числом.", ConsoleColor.Red);
		}
	}
	public int PositiveNumeric(string text) => NumericWithMin(text, min: 0);
}
