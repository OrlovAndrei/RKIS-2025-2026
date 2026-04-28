using ConsoleApp.Output.Interfaces;
using Spectre.Console;

namespace ConsoleApp.Input.Implementation;

internal class When(IColoredOutput coloredOutput)
{
	private readonly IColoredOutput _coloredOutput = coloredOutput;
	private readonly Text _text = new(coloredOutput);
	private readonly Numeric _numeric = new(coloredOutput);
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
			options: new[] { manual, pointBy, none });
		return res switch
		{
			manual => EInputMethod.Manual,
			pointBy => EInputMethod.PointBy,
			_ => null
		};
	}
	private void MessageIfDateIsNull(DateTime? dateTime)
	{
		if (dateTime is null)
		{
			_coloredOutput.WriteColoredLine("Вы не выбрали режим, дата по default будет 'Null'", ConsoleColor.Yellow);
		}
	}
	private void WarningAboutErrorFormatter(string userInputText)
	{
		_coloredOutput.WriteColoredLine($"'{userInputText}' не может быть преобразовано,", ConsoleColor.Red);
		_coloredOutput.WriteColoredLine($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
	}
	public DateTime ManualDate()
	{
		string exampleDate = DateTime.Now.ToShortDateString();
		string dateString;
		while (true)
		{
			dateString = _text.ShortText($"Введите дату (Пример {exampleDate}): ");
			if (DateTime.TryParse(dateString, out DateTime date))
			{
				return date;
			}
			WarningAboutErrorFormatter(dateString);
		}
	}
	public DateTime ManualTime()
	{
		string exampleDate = DateTime.Now.ToShortTimeString();
		string timeString;
		while (true)
		{
			timeString = _text.ShortText($"Введите время (Пример {exampleDate}): ");
			if (DateTime.TryParse(timeString, out DateTime time))
			{
				return time;
			}
			WarningAboutErrorFormatter(timeString);
		}
	}
	public DateTime PointByPointDate()
	{
		int year = _numeric.NumericWithMinMax("Введите год: ", 1, 9999);
		int month = _numeric.NumericWithMinMax("Введите месяц: ", 1, 12);
		int day = _numeric.NumericWithMinMax("Введите день: ", 1,
			DateTime.DaysInMonth(year, month));
		return new(year, month, day);
	}
	public DateTime PointByPointTime()
	{
		int hour = _numeric.NumericWithMinMax("Введите час: ", 0, 23);
		int minute = _numeric.NumericWithMinMax("Введите минуты: ", 0, 59);
		return new DateTime(1, 1, 1, hour, minute, 0);
	}
	public DateTime? DateAndTime(string? message)
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
	public DateTime? Date(string? message)
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
	public DateTime? Time(string? message)
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
