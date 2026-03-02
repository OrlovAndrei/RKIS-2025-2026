using ConsoleApp.Input.Interfaces;
using ConsoleApp.Output;
using ConsoleApp.Output.Interfaces;

namespace ConsoleApp.Adapters;

/// <summary>
/// Адаптер для работы только с вводом информации.
/// Реализует интерфейсы ввода текста, чисел, паролей и кнопок.
/// </summary>
public class InputAdapter : IInputProvider, ITextInput, INumericInput, IPasswordInput, IButtonInput
{
	/// <summary>
	/// Интерфейс ввода текста.
	/// </summary>
	private readonly ITextInput _textInput;
	/// <summary>
	/// Интерфейс ввода чисел.
	/// </summary>
	private readonly INumericInput _numericInput;
	/// <summary>
	/// Интерфейс ввода паролей.
	/// </summary>
	private readonly IPasswordInput _passwordInput;
	/// <summary>
	/// Интерфейс ввода кнопок.
	/// </summary>
	private readonly IButtonInput _buttonInput;
	/// <summary>
	/// Интерфейс цветного вывода.
	/// </summary>
	private readonly IColoredOutput _output;

	/// <summary>
	/// Конструктор адаптера ввода.
	/// </summary>
	/// <param name="output">Вывод информации (опционально).</param>
	/// <param name="textInput">Ввод текста (опционально).</param>
	/// <param name="numericInput">Ввод чисел (опционально).</param>
	/// <param name="passwordInput">Ввод паролей (опционально).</param>
	/// <param name="buttonInput">Ввод кнопок (опционально).</param>
	public InputAdapter(
		IColoredOutput? output = null,
		ITextInput? textInput = null,
		INumericInput? numericInput = null,
		IPasswordInput? passwordInput = null,
		IButtonInput? buttonInput = null
		)
	{
		_output = output ?? new ConsoleOutput();
		_textInput = textInput ?? GetDefaultTextInput();
		_numericInput = numericInput ?? GetDefaultNumericInput();
		_passwordInput = passwordInput ?? GetDefaultPasswordInput();
		_buttonInput = buttonInput ?? GetDefaultButtonInput();
	}

		#region ITextInput
		/// <summary>
		/// Получить короткий текст от пользователя.
		/// </summary>
		public string GetShortText(string prompt, bool notNull = true) => _textInput.GetShortText(prompt, notNull);

		/// <summary>
		/// Получить длинный текст от пользователя.
		/// </summary>
		public string GetLongText(string prompt) => _textInput.GetLongText(prompt);

		/// <summary>
		/// Получить непустой текст от пользователя.
		/// </summary>
		public string GetNonEmptyText(string prompt) => _textInput.GetNonEmptyText(prompt);
		#endregion

		#region INumericInput
		/// <summary>
		/// Получить числовое значение от пользователя.
		/// </summary>
		public int GetNumeric(string prompt) => _numericInput.GetNumeric(prompt);

		/// <summary>
		/// Получить числовое значение в заданном диапазоне.
		/// </summary>
		public int GetNumericInRange(string prompt, int min, int max) => _numericInput.GetNumericInRange(prompt, min, max);

		/// <summary>
		/// Получить числовое значение с минимальным ограничением.
		/// </summary>
		public int GetNumericWithMin(string prompt, int min) => _numericInput.GetNumericWithMin(prompt, min);

		/// <summary>
		/// Получить числовое значение с максимальным ограничением.
		/// </summary>
		public int GetNumericWithMax(string prompt, int max) => _numericInput.GetNumericWithMax(prompt, max);

		/// <summary>
		/// Получить положительное числовое значение.
		/// </summary>
		public int GetPositiveNumeric(string prompt) => _numericInput.GetPositiveNumeric(prompt);
		#endregion

		#region IPasswordInput
		/// <summary>
		/// Получить пароль от пользователя.
		/// </summary>
		public string GetPassword(string prompt) => _passwordInput.GetPassword(prompt);

		/// <summary>
		/// Получить пароль с подтверждением.
		/// </summary>
		public string GetCheckedPassword() => _passwordInput.GetCheckedPassword();

		/// <summary>
		/// Проверить длину пароля.
		/// </summary>
		/// <param name="password">Пароль.</param>
		/// <param name="minLength">Минимальная длина.</param>
		/// <returns>True, если длина достаточна.</returns>
		public bool ValidatePasswordLength(string password, int minLength = 8) => _passwordInput.ValidatePasswordLength(password, minLength);
		#endregion

		#region IButtonInput
		/// <summary>
		/// Получить выбор "Да/Нет" от пользователя.
		/// </summary>
		public bool GetYesNoChoice(string prompt) => _buttonInput.GetYesNoChoice(prompt);

		/// <summary>
		/// Получить выбор из списка.
		/// </summary>
		/// <param name="options">Список вариантов.</param>
		/// <param name="title">Заголовок (опционально).</param>
		/// <param name="pageSize">Размер страницы.</param>
		public string GetSelectionFromList(IEnumerable<string> options, string? title = null, int pageSize = 3) => _buttonInput.GetSelectionFromList(options, title, pageSize);

		/// <summary>
		/// Получить выбор из словаря.
		/// </summary>
		/// <param name="options">Словарь вариантов.</param>
		/// <param name="title">Заголовок (опционально).</param>
		/// <param name="pageSize">Размер страницы.</param>
		public KeyValuePair<int, string> GetSelectionFromDictionary(Dictionary<int, string> options, string? title = null, int pageSize = 3) => _buttonInput.GetSelectionFromDictionary(options, title, pageSize);

		/// <summary>
		/// Получить нажатую клавишу из набора.
		/// </summary>
		/// <param name="prompt">Подсказка.</param>
		/// <param name="defaultKey">Клавиша по умолчанию.</param>
		/// <param name="allowedKeys">Разрешённые клавиши.</param>
		public ConsoleKey GetKeyFromSet(string prompt, ConsoleKey defaultKey = ConsoleKey.Y, params ConsoleKey[] allowedKeys) => _buttonInput.GetKeyFromSet(prompt, defaultKey, allowedKeys);
		#endregion

		#region IInputProvider
		/// <summary>
		/// Получить текст от пользователя (реализация интерфейса).
		/// </summary>
		string IInputProvider.GetText(string prompt) => GetShortText(prompt);

		/// <summary>
		/// Получить число от пользователя (реализация интерфейса).
		/// </summary>
		int IInputProvider.GetNumeric(string prompt) => GetNumeric(prompt);

		/// <summary>
		/// Получить пароль от пользователя (реализация интерфейса).
		/// </summary>
		string IInputProvider.GetPassword(string prompt) => GetPassword(prompt);
		#endregion

		/// <summary>
		/// Получить стандартный интерфейс ввода текста.
		/// </summary>
		private ITextInput GetDefaultTextInput() => new Input.ConsoleInput(_output);

		/// <summary>
		/// Получить стандартный интерфейс ввода чисел.
		/// </summary>
		private INumericInput GetDefaultNumericInput() => new Input.ConsoleInput(_output);

		/// <summary>
		/// Получить стандартный интерфейс ввода паролей.
		/// </summary>
		private IPasswordInput GetDefaultPasswordInput() => new Input.ConsoleInput(_output);

		/// <summary>
		/// Получить стандартный интерфейс ввода кнопок.
		/// </summary>
		private IButtonInput GetDefaultButtonInput() => new Input.ConsoleInput(_output);
}
