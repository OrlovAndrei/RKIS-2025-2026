using Presentation.Input;

namespace Presentation.Output;

/// <summary>
/// Примеры использования системы ввода/вывода информации
/// </summary>
public static class InputOutputExamples
{
	/// <summary>
	/// Пример 1: Использование консольного вывода
	/// </summary>
	public static void ExampleConsoleOutput()
	{
		var output = new ConsoleOutput();
		
		// Простой текст
		output.WriteLine("Привет, мир!");
		output.WriteEmptyLine();
		
		// Цветной вывод
		output.WriteSuccess("Операция успешно выполнена!");
		output.WriteWarning("Это предупреждение");
		output.WriteInfo("Информационное сообщение");
		output.WriteError("Произошла ошибка!");
		
		// Вывод исключений
		try
		{
			throw new ArgumentException("Неверный аргумент");
		}
		catch (Exception ex)
		{
			output.WriteDetailedException(ex);
		}
	}

	/// <summary>
	/// Пример 2: Использование консольного ввода
	/// </summary>
	public static void ExampleConsoleInput()
	{
		var input = new ConsoleInput();
		var output = new ConsoleOutput();
		
		// Текстовый ввод
		string name = input.GetNonEmptyText("Введите ваше имя: ");
		output.WriteLine($"Привет, {name}!");
		
		// Числовой ввод
		int age = input.GetNumericInRange("Введите возраст (18-100): ", 18, 100);
		output.WriteLine($"Вам {age} лет");
		
		// Выбор да/нет
		if (input.GetYesNoChoice("Вы согласны с условиями?"))
		{
			output.WriteSuccess("Спасибо за согласие!");
		}
		
		// Выбор из списка
		var options = new[] { "Вариант 1", "Вариант 2", "Вариант 3" };
		string selected = input.GetSelectionFromList(options, "Выберите вариант:");
		output.WriteLine($"Вы выбрали: {selected}");
		
		// Пароль
		string password = input.GetCheckedPassword();
		output.WriteSuccess("Пароль установлен!");
	}

	/// <summary>
	/// Пример 3: Использование всех типов ввода
	/// </summary>
	public static void ExampleCompleteFlow()
	{
		var input = new ConsoleInput();
		var output = new ConsoleOutput();
		
		output.WriteInfo("=== Форма регистрации ===");
		output.WriteEmptyLine();
		
		// Текст
		string username = input.GetNonEmptyText("Имя пользователя: ");
		
		// Email (текст с валидацией)
		string email = input.GetNonEmptyText("Email: ");
		
		// Возраст (число в диапазоне)
		int age = input.GetNumericInRange("Возраст: ", 18, 120);
		
		// Пароль (с проверкой)
		string password = input.GetCheckedPassword();
		
		// Согласие
		bool agreeTerms = input.GetYesNoChoice("Согласны с условиями использования?");
		
		if (agreeTerms)
		{
			output.WriteSuccess("Регистрация завершена успешно!");
			output.WriteLine($"Пользователь: {username}");
			output.WriteLine($"Email: {email}");
			output.WriteLine($"Возраст: {age}");
		}
		else
		{
			output.WriteWarning("Регистрация отменена");
		}
	}
}
