using static ShevricTodo.Input.Button;
using static ShevricTodo.Input.Numeric;
using static ShevricTodo.Input.Text;
using static ShevricTodo.Input.WriteToConsole;
using static System.Console;

namespace ShevricTodo.Input;

internal class When
{
	public static DateTime ManualDate()
	{
		string exampleDate = DateTime.Now.ToShortDateString();
		string dateString;
		while (true)
		{
			dateString = ShortText($"Введите дату (Пример {exampleDate}): ");
			if (DateTime.TryParse(dateString, out DateTime date))
			{
				return date;
			}
			ColorMessage($"'{dateString}' не может быть преобразовано,", ConsoleColor.Red);
			ColorMessage($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
		}
	}
	public static DateTime ManualTime()
	{
		string exampleDate = DateTime.Now.ToShortTimeString();
		string timeString;
		while (true)
		{
			timeString = ShortText($"Введите время (Пример {exampleDate}): ");
			if (DateTime.TryParse(timeString, out DateTime time))
			{
				return time;
			}
			ColorMessage($"'{timeString}' не может быть преобразовано,", ConsoleColor.Red);
			ColorMessage($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
		}
	}
	public static DateTime PointByPointDate()
	{
		int year = NumericWithMinMax("Введите год: ", 1, 9999);
		int month = NumericWithMinMax("Введите месяц: ", 1, 12);
		int day = NumericWithMinMax("Введите день: ", 1,
			DateTime.DaysInMonth(year, month));
		return new(year, month, day);
	}
	public static DateTime PointByPointTime()
	{
		int hour = NumericWithMinMax("Введите час: ", 0, 23);
		int minute = NumericWithMinMax("Введите минуты: ", 0, 59);
		return new(year: 0, month: 0, day: 0, hour: hour, minute: minute, second: 0);
	}
	public static DateTime? DateAndTime(string? message)
	{
		WriteLine($"---Ввод даты и времени {message}---");
		OneOfButton($"Выберете метод ввода даты и времени: (Ручной('M'), Попунктный('P'))",
		out ConsoleKey key, ConsoleKey.M, ConsoleKey.P);
		DateTime? dateAndTime = key switch
		{
			ConsoleKey.P => Sum(PointByPointDate(), PointByPointTime()),
			ConsoleKey.M => Sum(ManualDate(), ManualTime()),
			_ => null
		};
		if (dateAndTime is null)
		{
			ColorMessage("Вы не выбрали режим, дата по default будет 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
		DateTime Sum(DateTime date, DateTime time)
		{
			return new DateTime(
				year: date.Year,
				month: date.Month,
				day: date.Day,
				hour: time.Hour,
				minute: time.Minute,
				second: time.Second);
		}
		// return ManualDate() + " " + ManualTime();
	}
	public static DateTime? Date(string? message)
	{
		/*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
		WriteLine($"---Ввод даты {message}---");
		OneOfButton($"Выберете метод ввода времени: (Ручной('M'), Попунктный('P'))",
		out ConsoleKey key, ConsoleKey.M, ConsoleKey.P);
		DateTime? dateAndTime = key switch
		{
			ConsoleKey.P => PointByPointDate(),
			ConsoleKey.M => ManualDate(),
			_ => null
		};
		if (dateAndTime is null)
		{
			ColorMessage("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
		// return ManualDate();
	}
	public static DateTime? Time(string? message)
	{
		/*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
		WriteLine($"---Ввод времени {message}---");
		OneOfButton($"Выберете метод ввода времени: (Ручной('M'), Попунктный('P'))",
		out ConsoleKey key, ConsoleKey.M, ConsoleKey.P);
		DateTime? dateAndTime = key switch
		{
			ConsoleKey.P => PointByPointTime(),
			ConsoleKey.M => ManualTime(),
			_ => null
		};
		if (dateAndTime is null)
		{
			ColorMessage("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
		}
		return dateAndTime;
	}
}
