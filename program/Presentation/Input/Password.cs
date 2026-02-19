using Spectre.Console;
using static Presentation.Input.WriteToConsole;

namespace Presentation.Input;

internal static class Password
{
	public static string GetPassword(string message)
	{
		var password = AnsiConsole.Prompt(
			new TextPrompt<string>(message)
				.Secret());
		return password;
	}
	private static (string password01, string password02) DoublePassword() =>
		(GetPassword("Введите пароль: "), GetPassword("Повторите пароль пароль: "));
	public static string CheckingThePassword()
	{
		string password01 = string.Empty;
		string password02 = string.Empty;
		(password01, password02) = DoublePassword();
		while (NotMatch() || NotAcceptableLength())
		{
			if (NotMatch()) { ColorMessage("Пароли не совпадают"); }
			if (NotAcceptableLength()) { ColorMessage("Пароль должен быть не менее 8 символов"); }
			(password01, password02) = DoublePassword();
		}
		return password01;
		bool NotMatch() => password01 != password02;
		bool NotAcceptableLength() => password01.Length < 8;
	}
}
