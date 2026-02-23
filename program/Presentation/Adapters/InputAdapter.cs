using Presentation.Input.Interfaces;

namespace Presentation.Adapters;

/// <summary>
/// Адаптер для работы только с вводом информации
/// </summary>
public class InputAdapter : IInputProvider, ITextInput, INumericInput, IPasswordInput, IButtonInput
{
	private readonly ITextInput _textInput;
	private readonly INumericInput _numericInput;
	private readonly IPasswordInput _passwordInput;
	private readonly IButtonInput _buttonInput;

	public InputAdapter(
		ITextInput? textInput = null,
		INumericInput? numericInput = null,
		IPasswordInput? passwordInput = null,
		IButtonInput? buttonInput = null)
	{
		_textInput = textInput ?? GetDefaultTextInput();
		_numericInput = numericInput ?? GetDefaultNumericInput();
		_passwordInput = passwordInput ?? GetDefaultPasswordInput();
		_buttonInput = buttonInput ?? GetDefaultButtonInput();
	}

	#region ITextInput
	public string GetShortText(string prompt, bool notNull = true) => _textInput.GetShortText(prompt, notNull);

	public string GetLongText(string prompt) => _textInput.GetLongText(prompt);

	public string GetNonEmptyText(string prompt) => _textInput.GetNonEmptyText(prompt);
	#endregion

	#region INumericInput
	public int GetNumeric(string prompt) => _numericInput.GetNumeric(prompt);

	public int GetNumericInRange(string prompt, int min, int max) => _numericInput.GetNumericInRange(prompt, min, max);

	public int GetNumericWithMin(string prompt, int min) => _numericInput.GetNumericWithMin(prompt, min);

	public int GetNumericWithMax(string prompt, int max) => _numericInput.GetNumericWithMax(prompt, max);

	public int GetPositiveNumeric(string prompt) => _numericInput.GetPositiveNumeric(prompt);
	#endregion

	#region IPasswordInput
	public string GetPassword(string prompt) => _passwordInput.GetPassword(prompt);

	public string GetCheckedPassword() => _passwordInput.GetCheckedPassword();

	public bool ValidatePasswordLength(string password, int minLength = 8) => _passwordInput.ValidatePasswordLength(password, minLength);
	#endregion

	#region IButtonInput
	public bool GetYesNoChoice(string prompt) => _buttonInput.GetYesNoChoice(prompt);

	public string GetSelectionFromList(IEnumerable<string> options, string? title = null, int pageSize = 3) => _buttonInput.GetSelectionFromList(options, title, pageSize);

	public KeyValuePair<int, string> GetSelectionFromDictionary(Dictionary<int, string> options, string? title = null, int pageSize = 3) => _buttonInput.GetSelectionFromDictionary(options, title, pageSize);

	public ConsoleKey GetKeyFromSet(string prompt, ConsoleKey defaultKey = ConsoleKey.Y, params ConsoleKey[] allowedKeys) => _buttonInput.GetKeyFromSet(prompt, defaultKey, allowedKeys);
	#endregion

	#region IInputProvider
	string IInputProvider.GetText(string prompt) => GetShortText(prompt);

	int IInputProvider.GetNumeric(string prompt) => GetNumeric(prompt);

	string IInputProvider.GetPassword(string prompt) => GetPassword(prompt);
	#endregion

	private static ITextInput GetDefaultTextInput() => new Input.ConsoleInput();

	private static INumericInput GetDefaultNumericInput() => new Input.ConsoleInput();

	private static IPasswordInput GetDefaultPasswordInput() => new Input.ConsoleInput();

	private static IButtonInput GetDefaultButtonInput() => new Input.ConsoleInput();
}
