using Presentation.Input.Interfaces;
using Presentation.Output.Interfaces;
using Presentation.Input;
using Presentation.Output;

namespace Presentation.Adapters;

/// <summary>
/// Фасад для объединенной работы с вводом и выводом информации
/// </summary>
public class IOFacade
{

	/// <summary>
	/// Инициализирует фасад с провайдерами ввода и вывода
	/// </summary>
	public IOFacade(IInputProvider? input = null, IErrorOutput? output = null)
	{
		Input = input ?? new ConsoleInput(output ?? new ConsoleOutput());
		Output = output ?? new ConsoleOutput();
	}

	/// <summary>
	/// Получить провайдер ввода
	/// </summary>
	public IInputProvider Input { get; }

	/// <summary>
	/// Получить провайдер вывода
	/// </summary>
	public IErrorOutput Output { get; }

	/// <summary>
	/// Вывести вопрос и получить строку
	/// </summary>
	public string AskText(string question, bool notNull = true)
	{
		Output.WriteLine(question);
		return (Input as ITextInput)?.GetShortText("", notNull) ?? "";
	}

	/// <summary>
	/// Вывести вопрос и получить число
	/// </summary>
	public int AskNumeric(string question)
	{
		Output.WriteLine(question);
		return Input.GetNumeric("");
	}

	/// <summary>
	/// Вывести вопрос и получить да/нет
	/// </summary>
	public bool AskYesNo(string question) => (Input as IButtonInput)?.GetYesNoChoice(question) ?? false;

	/// <summary>
	/// Вывести вопрос и получить выбор из списка
	/// </summary>
	public string AskSelection(string question, IEnumerable<string> options, int pageSize = 3)
	{
		Output.WriteLine(question);
		return (Input as IButtonInput)?.GetSelectionFromList(options, pageSize: pageSize) ?? "";
	}

	/// <summary>
	/// Вывести ошибку
	/// </summary>
	public void ShowError(string error) => Output.WriteError(error);

	/// <summary>
	/// Вывести успешное сообщение
	/// </summary>
	public void ShowSuccess(string message) => Output.WriteSuccess(message);

	/// <summary>
	/// Вывести предупреждение
	/// </summary>
	public void ShowWarning(string message) => Output.WriteWarning(message);

	/// <summary>
	/// Вывести информацию
	/// </summary>
	public void ShowInfo(string message) => Output.WriteInfo(message);

	/// <summary>
	/// Вывести исключение
	/// </summary>
	public void ShowException(Exception exception) => Output.WriteDetailedException(exception);

	/// <summary>
	/// Вывести разделитель
	/// </summary>
	public void ShowSeparator(string title = "")
	{
		Output.WriteEmptyLine();
		if (!string.IsNullOrEmpty(title))
		{
			Output.WriteInfo($"=== {title} ===");
		}
		else
		{
			Output.WriteInfo("===");
		}
		Output.WriteEmptyLine();
	}
}
