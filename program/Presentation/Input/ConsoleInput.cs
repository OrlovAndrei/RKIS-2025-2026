using Presentation.Input.Implementation;
using Presentation.Input.Interfaces;
using Presentation.Output.Interfaces;

namespace Presentation.Input;

/// <summary>
/// Реализация провайдера ввода информации через консоль
/// </summary>
public class ConsoleInput : IButtonInput, ITextInput, INumericInput, IPasswordInput, IInputProvider
{
	private readonly IColoredOutput? _output;
	private readonly Text _text;
	private readonly Numeric _numeric;
	private readonly Password _password;
	private readonly When _when;
	private readonly Button _button;

	public ConsoleInput(IColoredOutput output)
	{
		_output = output;
		_text = new Text(_output);
		_numeric = new Numeric(_output);
		_password = new Password(_output);
		_when = new When(_output);
		_button = new Button(_output);
	}

	#region ITextInput Members

	public string GetShortText(string prompt, bool notNull = true) => _text.ShortText(prompt, notNull);

	public string GetLongText(string prompt) => _text.LongText(prompt);

	public string GetNonEmptyText(string prompt) => _text.ShortText(prompt, notNull: true);

	#endregion

	#region INumericInput Members

	public int GetNumeric(string prompt) => _numeric.OneNumeric(prompt);

	public int GetNumericInRange(string prompt, int min, int max) => _numeric.NumericWithMinMax(prompt, min, max);

	public int GetNumericWithMin(string prompt, int min) => _numeric.NumericWithMin(prompt, min);

	public int GetNumericWithMax(string prompt, int max) => _numeric.NumericWithMax(prompt, max);

	public int GetPositiveNumeric(string prompt) => _numeric.PositiveNumeric(prompt);

	#endregion

	#region IPasswordInput Members

	public string GetPassword(string prompt) => Password.GetPassword(prompt);

	public string GetCheckedPassword() => _password.CheckingThePassword();

	public bool ValidatePasswordLength(string password, int minLength = 8) => password.Length >= minLength;

	#endregion

	#region IButtonInput Members

	public bool GetYesNoChoice(string prompt) => _button.YesOrNo(prompt);

	public string GetSelectionFromList(IEnumerable<string> options, string? title = null, int pageSize = 3) => OneOf.GetOneFromList(title: title, pageSize: pageSize, options: options);

	public KeyValuePair<int, string> GetSelectionFromDictionary(Dictionary<int, string> options, string? title = null, int pageSize = 3) => OneOf.GetOneFromList(options: options, title: title, pageSize: pageSize);

	public ConsoleKey GetKeyFromSet(string prompt, ConsoleKey defaultKey = ConsoleKey.Y, params ConsoleKey[] allowedKeys)
	{
		_button.OneOfButton(prompt, out ConsoleKey result, defaultKey, allowedKeys);
		return result;
	}

	#endregion

	#region IInputProvider Members

	string IInputProvider.GetText(string prompt) => GetShortText(prompt);

	int IInputProvider.GetNumeric(string prompt) => GetNumeric(prompt);

	string IInputProvider.GetPassword(string prompt) => GetPassword(prompt);

	#endregion
}
