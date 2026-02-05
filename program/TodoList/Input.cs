using System.Text;
using static System.Console;
using System.Security.Cryptography;
using Spectre.Console;
using static TodoList.WriteToConsole;
namespace TodoList;

/// <summary>
/// Опрос пользователя и ввод данных
/// </summary>
internal static class Input
{
	public static string GetOneFromList(List<string> option)
	{
		string res = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Выберите один из [green]вариантов[/]:")
				.PageSize(3)
				// .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
				.AddChoices(option));
		return res;
	}
	/// <summary>
	/// Опрашивает пользователя о типе данных
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <returns>Название типа данных</returns>
	public static string DataType(string text)
	{
		while (true)
		{
			string input = String(text);
			string res = SearchDataTypeOnJson.ConvertingInputValues(input);
			if (res.Length != 0)
			{
				return res;
			}
			ColorMessage("Вы ввели неподдерживаемый тип данных", ConsoleColor.Red);
		}
	}
	/// <summary>
	/// Ввод многострочных значений
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <returns>Одна большая строка</returns>
	public static string LongString(string text)
	{
		string endLine = @"\end";
		string inputChar = "> ";
		List<string> stringOutList = new();
		WriteLine(text);
		ColorMessage($"Введите '{endLine}', для окончания ввода", ConsoleColor.Green);
		while (true)
		{
			string input = String(inputChar, false);
			if (input != endLine)
			{
				stringOutList.Add(input);
			}
			else { break; }
		}
		return string.Join(" ", stringOutList.ToArray()); ;
	}
	/// <summary>
	/// Однострочный ввод строки
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <param name="notNull">Не допускается ли null(при значении false позволяет 
	/// ввести пустую строку)</param>
	/// <returns>Строка готовая к использованию</returns>
	public static string String(string text, bool notNull = true)
	{
		StringBuilder input = new();
		while (true)
		{
			Write(text);
			input.Append((ReadLine() ?? "").Trim());
			if (notNull)
			{
				if (input.ToString().Length != 0)
				{
					return input.ToString();
				}
				ColorMessage("Строка не должна быть пустой", ConsoleColor.Red);
			}
			else { return input.ToString(); }
		}
	}
	/// <summary>
	/// Ввод целого числа с границами допустимых значений
	/// </summary>
	/// <param name="text">Выводимое сообщение</param>
	/// <param name="min">Минимум</param>
	/// <param name="max">Максимум</param>
	/// <returns>Целочисленное значение соответствующие заданным границам</returns>
	public static int IntegerWithMinMax(string text, int min, int max)
	{
		int result;
		string input;
		while (true)
		{
			input = String(text);
			if (int.TryParse(input, out result) &&
			result >= min && result <= max)
			{
				return result;
			}
			ColorMessage($"'{input}' должно являться целым числом,", ConsoleColor.Red);
			ColorMessage($"быть меньше или равно (<=) {min},", ConsoleColor.Red);
			ColorMessage($"быть больше или равно (>=) {max}.", ConsoleColor.Red);
		}
	}
	public static int Integer(string text)
	{
		int result;
		while (true)
		{
			string input = String(text);
			if (int.TryParse(input, out result))
			{
				return result;
			}
			ColorMessage($"'{input}' должно являться целым числом.", ConsoleColor.Red);
		}
	}
	public static int PositiveInteger(string text)
	{
		int result;
		while (true)
		{
			string input = String(text);
			if (int.TryParse(input, out result) && result >= 0)
			{
				return result;
			}
			ColorMessage($"'{input}' должно являться целым числом,", ConsoleColor.Red);
			ColorMessage($"быть больше или равняться (>=) 0.", ConsoleColor.Red);
		}
	}
	public static float Float(string text)
	{
		float result;
		while (true)
		{
			string input = String(text);
			if (float.TryParse(input, out result))
			{
				return result;
			}
			ColorMessage($"'{input}' должно являться десятичным числом.", ConsoleColor.Red);
		}
	}
	public static float PositiveFloat(string text)
	{
		float result;
		while (true)
		{
			string input = String(text);
			if (float.TryParse(input, out result) && result >= 0)
			{
				return result;
			}
			ColorMessage($"'{input}' должно являться десятичным числом,", ConsoleColor.Red);
			ColorMessage($"быть больше или равняться (>=) 0.", ConsoleColor.Red);
		}
	}
	public static string ManualDate()
	{
		string exampleDate = DateTime.Now.ToShortDateString();
		string dateString;
		DateOnly dateOnly;
		while (true)
		{
			dateString = String($"Введите дату (Пример {exampleDate}): ");
			if (DateOnly.TryParse(dateString, out dateOnly))
			{
				return dateOnly.ToShortDateString();
			}
			ColorMessage($"'{dateString}' не может быть преобразовано,", ConsoleColor.Red);
			ColorMessage($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
		}
	}
	public static string ManualTime()
	{
		string exampleDate = DateTime.Now.ToShortTimeString();
		string timeString;
		TimeOnly timeOnly;
		while (true)
		{
			timeString = String($"Введите время (Пример {exampleDate}): ");
			if (TimeOnly.TryParse(timeString, out timeOnly))
			{
				return timeOnly.ToShortTimeString();
			}
			ColorMessage($"'{timeString}' не может быть преобразовано,", ConsoleColor.Red);
			ColorMessage($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
		}
	}
	public static string PointByPointDate()
	{
		int year = IntegerWithMinMax("Введите год: ", 1, 9999);
		int month = IntegerWithMinMax("Введите месяц: ", 1, 12);
		int day = IntegerWithMinMax("Введите день: ", 1,
			DateTime.DaysInMonth(year, month));
		DateOnly yearMonthDay = new(year, month, day);
		return yearMonthDay.ToShortDateString();
	}
	public static string PointByPointTime()
	{
		int hour = IntegerWithMinMax("Введите час: ", 0, 23);
		int minute = IntegerWithMinMax("Введите минуты: ", 0, 59);
		TimeOnly hourAndMinute = new(hour, minute);
		return hourAndMinute.ToShortTimeString();
	}
	public static string? DateAndTime(string? message)
	{
		WriteLine($"---Ввод даты и времени {message}---");
		Key($"Выберете метод ввода даты и времени: (Ручной('M'), Попунктный('P'))",
		out ConsoleKey key, ConsoleKey.M, ConsoleKey.P);
		string? dateAndTime = key switch
		{
			ConsoleKey.P => PointByPointDate() + " " + PointByPointTime(),
			ConsoleKey.M => ManualDate() + " " + ManualTime(),
			_ => null
		};
		if (dateAndTime is null || dateAndTime.Length == 0)
		{
			ColorMessage("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
		// return ManualDate() + " " + ManualTime();
	}
	public static string? Date(string? message)
	{
		/*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
		WriteLine($"---Ввод даты {message}---");
		Key($"Выберете метод ввода времени: (Ручной('M'), Попунктный('P'))",
		out ConsoleKey key, ConsoleKey.M, ConsoleKey.P);
		string? dateAndTime = key switch
		{
			ConsoleKey.P => PointByPointDate(),
			ConsoleKey.M => ManualDate(),
			_ => null
		};
		if (dateAndTime is null || dateAndTime.Length == 0)
		{
			ColorMessage("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
		// return ManualDate();
	}
	public static string? Time(string? message)
	{
		/*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
		WriteLine($"---Ввод времени {message}---");
		Key($"Выберете метод ввода времени: (Ручной('M'), Попунктный('P'))",
		out ConsoleKey key, ConsoleKey.M, ConsoleKey.P);
		string? dateAndTime = key switch
		{
			ConsoleKey.P => PointByPointTime(),
			ConsoleKey.M => ManualTime(),
			_ => null
		};
		if (dateAndTime is null || dateAndTime.Length == 0)
		{
			ColorMessage("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
	}
	public static string NowDateTime()
	{
		/*возвращает сегодняшнюю дату и время в нужном формате*/
		DateTime nowDate = DateTime.Now;
		return nowDate.ToShortDateString() +
			" " + nowDate.ToShortTimeString();
	}
	public static void IfNull(string writeText, ref string? text)
	{
		if (text is null || text.Length == 0)
		{
			text = String(writeText);
		}
	}
	public static void IfNullOnDataType(CSVFile fileCSV, int index, ref string? text)
	{
		if (text is null || text.Length == 0)
		{
			text = StringOnTitleAndConfig(fileCSV, index);
		}
	}
	public static void GetCSVLine(CSVFile fileCSV, out CSVLine outLine)
	{
		outLine = new();
		for (int i = 0; i < fileCSV.Title!.Length(); i++)
		{
			outLine.Items.Add(StringOnTitleAndConfig(fileCSV, i));
		}
	}
	public static string? StringOnTitleAndConfig(CSVFile fileCSV, int index)
	{
		return fileCSV.DataType![index] switch
		{
			"lb" => IntToBool(Survey.resultOperation),

			"s" => String($"Введите {fileCSV.Title![index]} (string): "),

			"ls" => LongString($"Введите {fileCSV.Title![index]} (long string): "),

			"i" => Integer($"Введите {fileCSV.Title![index]} (int): ").ToString(),

			"pos_i" => PositiveInteger($"Введите {fileCSV.Title![index]} (pos. int): ").ToString(),

			"f" => Float($"Введите {fileCSV.Title![index]} (float): ").ToString(),

			"pos_f" => PositiveFloat($"Введите {fileCSV.Title![index]} (pos. float): ").ToString(),

			"d" => Date(fileCSV.Title![index]),

			"t" => Time(fileCSV.Title![index]),

			"dt" => DateAndTime(fileCSV.Title![index]),

			"ndt" => NowDateTime(),

			"false" => false.ToString(),

			"true" => true.ToString(),

			"b" => Bool($"Введите {fileCSV.Title![index]} (bool): ").ToString(),

			"counter" => (fileCSV.File.Length() + 1).ToString(), //+1 для того что бы счетчик не начинался с 0 элемента

			"prof" => Commands.SearchActiveProfile()[2],

			"command" when Survey.CommandLineGlobal != null => Survey.CommandLineGlobal.Command,

			"option" when Survey.CommandLineGlobal != null => string.Join(",", Survey.CommandLineGlobal.Options!),

			"textline" when Survey.CommandLineGlobal != null => Survey.CommandLineGlobal.Argument,

			"status" when fileCSV.File.NameFile.Equals(Task.Pattern.File.NameFile) => GetOneFromList(Task.Status),

			"command" => "",

			"option" => "",

			"textline" => "",

			"pas" => CreateNewPasswordSHA256(),

			"ruid" => CreateUID().ToString(),

			"uid" => TodoList.Password.GetUIDWithoutPassword(),

			"auid" => GetActiveUID(),

			_ => null
		};
	}
	public static string GetActiveUID()
    {
		return Profile.Pattern.File.SearchLineOnDataInLine
			(requiredData: true.ToString(),
			indexInLine: 1).Objects[0][0];
		
    }
	public static string CreateMD5(string input)
	{
#pragma warning disable IDE1006 // Стили именования
		MD5 MD5Hash = MD5.Create(); //создаем объект для работы с MD5
#pragma warning restore IDE1006 // Стили именования
		byte[] inputBytes = Encoding.ASCII.GetBytes(input); //преобразуем строку в массив байтов
		byte[] hash = MD5Hash.ComputeHash(inputBytes); //получаем хэш в виде массива байтов
		return Convert.ToHexString(hash); //преобразуем хэш из массива в строку, состоящую из шестнадцатеричных символов в верхнем регистре
	}
	private static string CreateNewPasswordSHA256() =>
		CreateSHA256(CheckingThePassword() + TodoList.Password.GetUIDWithoutPassword());
	public static string CreatePasswordSHA256(string uid) =>
		CreateSHA256(Password("Введите пароль: ") + uid);
	public static string CreateSHA256(string input)
	{
		using SHA256 hash = SHA256.Create();
		return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(input)));
	}
	private static string NowDateForHash() => $"{DateTime.Now:yyyyMMddHHmmssffffff}";
	private static int RandomSeed()
	{
		Random random = new Random();
		int.TryParse(NowDateForHash(), out int seed);
		seed += random.Next();
		return seed;
	}
	private static Int64 CreateUID()
	{
		Random random = new Random(RandomSeed());
		return random.NextInt64();
	}
	private static string CreateUIDasMD5() => CreateMD5(CreateUID().ToString());
	private static string IntToBool(int num)
	{
		if (num == 1)
		{
			return "OK";
		}
		return "FAIL";
	}
	public static bool Bool(string text,
	ConsoleKey yes = ConsoleKey.Y, ConsoleKey no = ConsoleKey.N)
	{
		Key(text, out ConsoleKey key, yes, no);
		switch (key)
		{
			case ConsoleKey.Y:
				return true;
			default:
				return false;
		}
	}
	public static void Key(string text, out ConsoleKey key,
	ConsoleKey standard = ConsoleKey.Y, params ConsoleKey[] keys)
	{
		List<string> allKey = [standard.ToString().ToUpper()];
		foreach (ConsoleKey keySmall in keys)
		{
			allKey.Add(keySmall.ToString().ToLower());
		}
		Write($"{text} ({string.Join("/", allKey)}): ");
		ConsoleKeyInfo keyInput = ReadKey();
		key = standard;
		foreach (ConsoleKey keySmall in keys)
		{
			if (keyInput.Key == keySmall)
			{
				key = keyInput.Key;
				break;
			}
		}
		WriteLine();
	}
	public static int WriteColumn(string fileName, int start = 0)
	{
		CSVFile fileCSV = new(fileName);
		string[] option = fileCSV.Title![start..].ToArray()!;
		var res = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Выберите в каком [green]столбце[/] проводить поиски?")
				.PageSize(10)
				// .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
				.AddChoices(option));
		for (int i = start; i < fileCSV.Title.Length(); ++i)
		{
			if (res == fileCSV.Title[i])
			{
				return i;
			}
		}
		return start;
	}
	public static int WriteColumn(List<string> list, int start = 0)
	{
		string[] option = list[start..].ToArray()!;
		var res = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Выберите в каком [green]столбце[/] проводить поиски?")
				.PageSize(10)
				// .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
				.AddChoices(option));
		for (int i = start; i < list.Count(); ++i)
		{
			if (res == list[i])
			{
				return i;
			}
		}
		return start;
	}
	public static string GetStringWriteColumn(List<string> list) =>
		list[WriteColumn(list)];

	public static string Password(string message)
	{
		var password = AnsiConsole.Prompt(
			new TextPrompt<string>(message)
				.Secret());
		return password;
	}
	private static (string password01, string password02) DoublePassword() =>
		(Password("Введите пароль: "), Password("Повторите пароль пароль: "));
	public static string CheckingThePassword()
	{
		(string password01, string password02) = DoublePassword();
		bool NotMatch() => password01 != password02;
		bool NotAcceptableLength() => password01.Length < 8 && password02.Length < 8;
		while (NotMatch() || NotAcceptableLength())
		{
			if (NotMatch()) { ColorMessage("Пароли не совпадают"); }
			if (NotAcceptableLength()) { ColorMessage("Пароль должен быть не менее 8 символов"); }
			(password01, password02) = DoublePassword();
		}
		return password01;
	}
}
public static class WriteToConsole
{
	public static void ColorMessage(string textError, ConsoleColor colorText = ConsoleColor.Red)
	{
		ForegroundColor = colorText;
		WriteLine(textError);
		ResetColor();
	}
	public static void Text(params string[] text)
	{
		foreach (string textItem in text)
		{
			ColorMessage(textItem, ConsoleColor.DarkYellow);
		}
	}
	public static void ProcExcept(Exception ex)
	{
		ColorMessage($"Исключение: {ex.Message}", ConsoleColor.Red);
		ColorMessage($"Метод: {ex.TargetSite}", ConsoleColor.Red);
		ColorMessage($"Трассировка стека: {ex.StackTrace}", ConsoleColor.DarkYellow);
		if (ex.InnerException is not null)
		{
			ColorMessage($"{ex.InnerException}", ConsoleColor.Yellow);
		}
	}
}