using System.Text;
using static System.Console;
using static Task.WriteToConsole;
namespace Task;

internal static class Input
{
	public static string DataType(string text)
	{
		/*Выводит на экран текст и запрашивает у пользователя 
            ввести тип данных и вводит его в бесконечный цикл 
            вводимая пользователем строка проверяеться на наличие 
            такого типа и если он есть возвращает его сокращение*/
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
	public static string LongString(string text)
	{
		List<string> stringOutList = new();
		WriteLine(text);
		while (true)
		{
			string input = String(Const.PrintInTerminal, false);
			if (input != "\\end")
			{
				stringOutList.Add(input);
			}
			else { break; }
		}
		return string.Join(" ", stringOutList.ToArray()); ;
	}
	public static string String(string text, bool notNull = true)
	{
		/*выводит текст пользователю и запрашивает 
            ввести строковые данные, они проверяются на
            наличие и если строка пуста то возвращаеться 
            "NULL" если нет то возвращается обработаная 
            версия строки*/
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
	public static int IntegerWithMinMax(string text, int min, int max)
	{
		/*Запрашивает у пользователя дату, проверяется
            на минимальное и максимальное допустимое значение,
            а так же возвращает простые цифры с нулем.
            Пример: 02, 00, 09 и тд.s*/
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
	public static string? DateAndTime(string message)
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
	public static string? Date(string message)
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
	public static string? Time(string message)
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
		if (text == null || text.Length == 0)
		{
			text = String(writeText);
		}
	}
	public static string RowOnTitleAndConfig(string[] titleRowArray, string[] dataTypeRowArray, string nameData)
	{
		FormatterRows row = new(nameData);
		for (int i = 0; i < titleRowArray.Length; i++)
		{
			if (dataTypeRowArray[i] == "counter" || dataTypeRowArray[i] == "bool") { continue; }
			row.AddInRow(dataTypeRowArray[i] switch
			{
				"s" => String($"введите {titleRowArray[i]} (string): "),
				"ls" => LongString($"введите {titleRowArray[i]} (long string): "),
				"i" => Integer($"введите {titleRowArray[i]} (int): ").ToString(),
				"pos_i" => PositiveInteger($"введите {titleRowArray[i]} (pos. int): ").ToString(),
				"f" => Float($"введите {titleRowArray[i]} (float): ").ToString(),
				"pos_f" => PositiveFloat($"введите {titleRowArray[i]} (pos. float): ").ToString(),
				"d" => Date(titleRowArray[i]),
				"t" => Time(titleRowArray[i]),
				"dt" => DateAndTime(titleRowArray[i]),
				"ndt" => NowDateTime(),
				"b" => Bool($"введите {titleRowArray[i]} (bool): ").ToString(),
				"prof" => Commands.SearchActiveProfile().Split(Const.SeparRows)[2],
				"command" when Survey.CommandLineGlobal != null => Survey.CommandLineGlobal.Command,
				"option" when Survey.CommandLineGlobal != null => string.Join(",", Survey.CommandLineGlobal.Options!),
				"textline" when Survey.CommandLineGlobal != null => Survey.CommandLineGlobal.Argument,
				"command" => "",
				"option" => "",
				"textline" => "",
				_ => null
			});
		}
		return row.GetRow();
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
}
public class WriteToConsole
{
	public static void RainbowText(string textError, ConsoleColor colorText)
	{
		ForegroundColor = colorText;
		WriteLine(textError);
		ResetColor();
	}
	public static void Text(params string[] text)
	{
		foreach (string textItem in text)
		{
			WriteLine(textItem);
		}
	}
}