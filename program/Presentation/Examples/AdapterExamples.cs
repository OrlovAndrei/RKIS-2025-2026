using Presentation.Adapters;

namespace Presentation.Examples;

/// <summary>
/// Примеры использования Input/Output адаптеров и фасадов
/// </summary>
public static class AdapterExamples
{
	/// <summary>
	/// Пример 1: Использование IOFacade
	/// </summary>
	public static void ExampleIOFacade()
	{
		var facade = new IOFacade();

		facade.ShowInfo("Добро пожаловать!");
		facade.ShowSeparator("Форма ввода");

		string name = facade.AskText("Введите ваше имя:");
		int age = facade.AskNumeric("Введите возраст:");
		bool agree = facade.AskYesNo("Согласны с условиями?");

		if (agree)
		{
			facade.ShowSuccess($"Благодарим, {name}! Вам {age} лет.");
		}
		else
		{
			facade.ShowWarning("Операция отменена.");
		}
	}

	/// <summary>
	/// Пример 2: Использование InputAdapter
	/// </summary>
	public static void ExampleInputAdapter()
	{
		var input = new InputAdapter();

		// Работа с разными типами ввода
		string text = input.GetNonEmptyText("Текст: ");
		int number = input.GetNumericInRange("Число (1-10): ", 1, 10);
		int positive = input.GetPositiveNumeric("Положительное число: ");
		bool choice = input.GetYesNoChoice("Да/Нет: ");
		string password = input.GetPassword("Пароль: ");

		Console.WriteLine($"Результаты: {text}, {number}, {positive}, {choice}, {'*'}");
	}

	/// <summary>
	/// Пример 3: Использование OutputAdapter
	/// </summary>
	public static void ExampleOutputAdapter()
	{
		var output = new OutputAdapter();

		output.WriteInfo("=== Демонстрация OutputAdapter ===");
		output.WriteEmptyLine();

		// Простой вывод
		output.WriteLine("Обычное сообщение");
		output.WriteSuccess("Успешно!");
		output.WriteWarning("Предупреждение!");
		output.WriteError("Ошибка!");
		output.WriteInfo("Информация");
		output.WriteEmptyLine();

		// Меню
		output.WriteMenu("Выберите действие:",
			("1", "Опция 1"),
			("2", "Опция 2"),
			("3", "Опция 3"),
			("0", "Выход")
		);

		// Список
		output.WriteLine("Список товаров:");
		output.WriteList(
			new[] { "Яблоки", "Апельсины", "Бананы" },
			item => $"Продукт: {item}"
		);
		output.WriteEmptyLine();

		// Таблица
		output.WriteLine("Таблица пользователей:");
		var users = new[]
		{
			new { Name = "Иван", Age = 25 },
			new { Name = "Мария", Age = 30 },
			new { Name = "Петр", Age = 35 }
		};
		output.WriteTable(users, user => $"{user.Name,-15} {user.Age,3} лет");
	}

	/// <summary>
	/// Пример 4: Комплексная форма с валидацией
	/// </summary>
	public static void ExampleComplexForm()
	{
		var io = new IOFacade();

		io.ShowSeparator("Регистрация пользователя");

		try
		{
			// Получить данные
			string username = io.AskText("Имя пользователя (не менее 3 символов):");
			if (username.Length < 3)
			{
				io.ShowError("Имя должно содержать минимум 3 символа!");
				return;
			}

			string email = io.AskText("Email:");
			if (!email.Contains("@"))
			{
				io.ShowError("Email должен содержать @!");
				return;
			}

			int age = io.AskNumeric("Возраст:");
			if (age < 18 || age > 120)
			{
				io.ShowError("Возраст должен быть от 18 до 120!");
				return;
			}

			bool agree = io.AskYesNo("Согласны с условиями использования?");
			if (!agree)
			{
				io.ShowWarning("Регистрация отменена.");
				return;
			}

			// Вывести результат
			io.ShowSeparator("Результаты");
			io.ShowSuccess("Регистрация успешно завершена!");
			io.Output.WriteLine($"Пользователь: {username}");
			io.Output.WriteLine($"Email: {email}");
			io.Output.WriteLine($"Возраст: {age}");
		}
		catch (Exception ex)
		{
			io.ShowException(ex);
		}
	}

	/// <summary>
	/// Пример 5: Работа с исключениями
	/// </summary>
	public static void ExampleErrorHandling()
	{
		var output = new OutputAdapter();

		output.WriteInfo("=== Обработка ошибок ===");
		output.WriteEmptyLine();

		try
		{
			// Имитируем ошибку
			int[] numbers = { 1, 2, 3 };
			_ = numbers[10];  // IndexOutOfRangeException
		}
		catch (Exception ex)
		{
			output.WriteError("Произошла ошибка:");
			output.WriteDetailedException(ex);
		}

		output.WriteEmptyLine();
		output.WriteWarning("Операция перехвачена и обработана.");
	}

	/// <summary>
	/// Пример 6: Интерактивное меню
	/// </summary>
	public static void ExampleInteractiveMenu()
	{
		var io = new IOFacade();

		io.ShowSeparator("Главное меню");

		var menuOptions = new[] { "Добавить", "Удалить", "Изменить", "Выход" };
		string selected = io.AskSelection("Выберите действие:", menuOptions);

		io.Output.WriteSuccess($"Вы выбрали: {selected}");
	}
}
