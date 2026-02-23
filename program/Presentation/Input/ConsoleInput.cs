using Presentation.Input.Interfaces;
using Presentation.Output.Interfaces;

namespace Presentation.Input;

/// <summary>
/// Реализация провайдера ввода информации через консоль
/// </summary>
public class ConsoleInput : IButtonInput, ITextInput, INumericInput, IPasswordInput, IInputProvider
{
	private readonly IColoredOutput? _output;

	public ConsoleInput(IColoredOutput? output = null)
	{
		_output = output;
	}

	#region ITextInput Members

	public string GetShortText(string prompt, bool notNull = true) => Text.ShortText(prompt, notNull);

	public string GetLongText(string prompt) => Text.LongText(prompt);

	public string GetNonEmptyText(string prompt) => Text.ShortText(prompt, notNull: true);

	#endregion

	#region INumericInput Members

	public int GetNumeric(string prompt) => Numeric.OneNumeric(prompt);

	public int GetNumericInRange(string prompt, int min, int max) => Numeric.NumericWithMinMax(prompt, min, max);

	public int GetNumericWithMin(string prompt, int min) => Numeric.NumericWithMin(prompt, min);

	public int GetNumericWithMax(string prompt, int max) => Numeric.NumericWithMax(prompt, max);

	public int GetPositiveNumeric(string prompt) => Numeric.PositiveNumeric(prompt);

	#endregion

	#region IPasswordInput Members

	public string GetPassword(string prompt) => Password.GetPassword(prompt);

	public string GetCheckedPassword() => Password.CheckingThePassword();

	public bool ValidatePasswordLength(string password, int minLength = 8) => password.Length >= minLength;

	#endregion

	#region IButtonInput Members

	public bool GetYesNoChoice(string prompt) => Button.YesOrNo(prompt);

	public string GetSelectionFromList(IEnumerable<string> options, string? title = null, int pageSize = 3) => OneOf.GetOneFromList(title: title, pageSize: pageSize, options: options);

	public KeyValuePair<int, string> GetSelectionFromDictionary(Dictionary<int, string> options, string? title = null, int pageSize = 3) => OneOf.GetOneFromList(options: options, title: title, pageSize: pageSize);

	public ConsoleKey GetKeyFromSet(string prompt, ConsoleKey defaultKey = ConsoleKey.Y, params ConsoleKey[] allowedKeys)
	{
		Button.OneOfButton(prompt, out ConsoleKey result, defaultKey, allowedKeys);
		return result;
	}

	#endregion

	#region IInputProvider Members

	string IInputProvider.GetText(string prompt) => GetShortText(prompt);

	int IInputProvider.GetNumeric(string prompt) => GetNumeric(prompt);

	string IInputProvider.GetPassword(string prompt) => GetPassword(prompt);

	#endregion
}
