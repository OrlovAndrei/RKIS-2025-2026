using System.Text;
using static System.Console;
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
			RainbowText("Вы ввели неподдерживаемый тип данных", ConsoleColor.Red);
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
		List<string> stringOutList = new();
		WriteLine(text);
		RainbowText($"Введите '{endLine}', для окончания ввода", ConsoleColor.Green);
		while (true)
		{
			string input = String(Const.PrintInTerminal, false);
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
				RainbowText("Строка не должна быть пустой", ConsoleColor.Red);
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
			RainbowText($"'{input}' должно являться целым числом,", ConsoleColor.Red);
			RainbowText($"быть меньше или равно (<=) {min},", ConsoleColor.Red);
			RainbowText($"быть больше или равно (>=) {max}.", ConsoleColor.Red);
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
			RainbowText($"'{input}' должно являться целым числом.", ConsoleColor.Red);
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
			RainbowText($"'{input}' должно являться целым числом,", ConsoleColor.Red);
			RainbowText($"быть больше или равняться (>=) 0.", ConsoleColor.Red);
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
			RainbowText($"'{input}' должно являться десятичным числом.", ConsoleColor.Red);
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
			RainbowText($"'{input}' должно являться десятичным числом,", ConsoleColor.Red);
			RainbowText($"быть больше или равняться (>=) 0.", ConsoleColor.Red);
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
			RainbowText($"'{dateString}' не может быть преобразовано,", ConsoleColor.Red);
			RainbowText($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
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
			RainbowText($"'{timeString}' не может быть преобразовано,", ConsoleColor.Red);
			RainbowText($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
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
		/*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
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
			RainbowText("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
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
			RainbowText("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
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
			RainbowText("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
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
	public static void RowOnTitleAndConfig(CSVFile fileCSV, out CSVLine outLine)
	{
		outLine = new();
		for (int i = 0; i < fileCSV.Title!.GetLength(); i++)
		{
			outLine.Items.Add(StringOnTitleAndConfig(fileCSV, i));
		}
	}
	public static string? StringOnTitleAndConfig(CSVFile fileCSV, int index)
    {
		return fileCSV.DataType![index] switch
		{
			"lb" => IntToBool(Survey.resultOperation).ToString(),
			"s" => String($"введите {fileCSV.Title![index]} (string): "),
			"ls" => LongString($"введите {fileCSV.Title![index]} (long string): "),
			"i" => Integer($"введите {fileCSV.Title![index]} (int): ").ToString(),
			"pos_i" => PositiveInteger($"введите {fileCSV.Title![index]} (pos. int): ").ToString(),
			"f" => Float($"введите {fileCSV.Title![index]} (float): ").ToString(),
			"pos_f" => PositiveFloat($"введите {fileCSV.Title![index]} (pos. float): ").ToString(),
			"d" => Date(fileCSV.Title![index]),
			"t" => Time(fileCSV.Title![index]),
			"dt" => DateAndTime(fileCSV.Title![index]),
			"ndt" => NowDateTime(),
			"false" => false.ToString(),
			"true" => true.ToString(),
			"b" => Bool($"введите {fileCSV.Title![index]} (bool): ").ToString(),
			"counter" => fileCSV.File.GetLengthFile().ToString(),
			"prof" => Commands.SearchActiveProfile()[2],
			"command" when Survey.CommandLineGlobal != null => Survey.CommandLineGlobal.Command,
			"option" when Survey.CommandLineGlobal != null => string.Join(",", Survey.CommandLineGlobal.Options!),
			"textline" when Survey.CommandLineGlobal != null => Survey.CommandLineGlobal.Argument,
			"status" when fileCSV.File.NameFile.Equals(Task.Pattern.File.NameFile) => GetOneFromList(Task.Status),
			"command" => "",
			"option" => "",
			"textline" => "",
			_ => null
		};
    }
	private static bool IntToBool(int num)
	{
		if (num == 1)
		{
			return true;
		}
		else
        {
			return false;
        }
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
		for (int i = start; i < fileCSV.Title.GetLength(); ++i)
		{
			if (res == fileCSV.Title[i])
			{
				return i;
			}
		}
		return start;
	}
}
public class WriteToConsole
{
	public static void RainbowText(string textError, ConsoleColor colorText = ConsoleColor.Red)
	{
		ForegroundColor = colorText;
		WriteLine(textError);
		ResetColor();
	}
	public static void Text(params string[] text)
	{
		foreach (string textItem in text)
		{
			RainbowText(textItem, ConsoleColor.DarkYellow);
		}
	}
}