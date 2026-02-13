using Spectre.Console;
using static ShevricTodo.Input.Numeric;
using static ShevricTodo.Input.Text;
using static ShevricTodo.Input.WriteToConsole;

namespace ShevricTodo.Input;

internal static class When
{
	private enum EInputMethod
	{
		Manual,
		PointBy
	}
	private static EInputMethod? GetEInputMethod()
	{
		const string manual = "Ручной";
		const string pointBy = "По пунктам";
		const string none = "Не вводить";
		string res = OneOf.GetOneFromList(
			title: "Выберете метод ввода даты и времени",
			options: [manual, pointBy, none]);
		return res switch
		{
			manual => EInputMethod.Manual,
			pointBy => EInputMethod.PointBy,
			_ => null
		};
	}
	private static void MessageIfDateIsNull(DateTime? dateTime)
	{
		if (dateTime is null)
		{
			ColorMessage("Вы не выбрали режим, дата по default будет 'Null'", ConsoleColor.Yellow);
		}
	}
	private static void WarningAboutErrorFormatter(string userInputText)
	{
		ColorMessage($"'{userInputText}' не может быть преобразовано,", ConsoleColor.Red);
		ColorMessage($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
	}
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
			WarningAboutErrorFormatter(dateString);
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
			WarningAboutErrorFormatter(timeString);
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
		if (message is not null) AnsiConsole.Write(new Rule(message));
		var mod = GetEInputMethod();
		DateTime? dateAndTime = mod switch
		{
			EInputMethod.PointBy => Sum(PointByPointDate(), PointByPointTime()),
			EInputMethod.Manual => Sum(ManualDate(), ManualTime()),
			_ => null
		};
		MessageIfDateIsNull(dateAndTime);
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
		if (message is not null) AnsiConsole.Write(new Rule(message));
		var mod = GetEInputMethod();
		DateTime? dateAndTime = mod switch
		{
			EInputMethod.PointBy => PointByPointDate(),
			EInputMethod.Manual => ManualDate(),
			_ => null
		};
		MessageIfDateIsNull(dateAndTime);
		return dateAndTime;
		// return ManualDate();
	}
	public static DateTime? Time(string? message)
	{
		/*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
		if (message is not null) AnsiConsole.Write(new Rule(message));
		var mod = GetEInputMethod();
		DateTime? dateAndTime = mod switch
		{
			EInputMethod.PointBy => PointByPointTime(),
			EInputMethod.Manual => ManualTime(),
			_ => null
		};
		MessageIfDateIsNull(dateAndTime);
		return dateAndTime;
	}
}
