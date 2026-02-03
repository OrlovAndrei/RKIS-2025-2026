using static System.Console;
using static ShevricTodo.Input.WriteToConsole;
using static ShevricTodo.Input.Text;
using static ShevricTodo.Input.Button;
using static ShevricTodo.Input.Numeric;

namespace ShevricTodo.Input;

internal class When
{
	public static string ManualDate()
	{
		string exampleDate = DateTime.Now.ToShortDateString();
		string dateString;
		DateOnly dateOnly;
		while (true)
		{
			dateString = ShortText($"Введите дату (Пример {exampleDate}): ");
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
			timeString = ShortText($"Введите время (Пример {exampleDate}): ");
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
		int year = NumericWithMinMax("Введите год: ", 1, 9999);
		int month = NumericWithMinMax("Введите месяц: ", 1, 12);
		int day = NumericWithMinMax("Введите день: ", 1,
			DateTime.DaysInMonth(year, month));
		DateOnly yearMonthDay = new(year, month, day);
		return yearMonthDay.ToShortDateString();
	}
	public static string PointByPointTime()
	{
		int hour = NumericWithMinMax("Введите час: ", 0, 23);
		int minute = NumericWithMinMax("Введите минуты: ", 0, 59);
		TimeOnly hourAndMinute = new(hour, minute);
		return hourAndMinute.ToShortTimeString();
	}
	public static string? DateAndTime(string? message)
	{
		WriteLine($"---Ввод даты и времени {message}---");
		OneOfButton($"Выберете метод ввода даты и времени: (Ручной('M'), Попунктный('P'))",
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
		OneOfButton($"Выберете метод ввода времени: (Ручной('M'), Попунктный('P'))",
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
		OneOfButton($"Выберете метод ввода времени: (Ручной('M'), Попунктный('P'))",
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
}
