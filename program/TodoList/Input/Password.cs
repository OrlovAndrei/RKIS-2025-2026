using System.Text;
using static System.Console;
using Spectre.Console;
using static ShevricTodo.Input.WriteToConsole;

namespace ShevricTodo.Input;

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
		(Password("Введите пароль: "), Password("Повторите пароль пароль: "));
	public static string CheckingThePassword()
	{
		(string password01, string password02) = DoublePassword();
		bool NotMatch() => password01 != password02;
		bool NotAcceptableLength() => password01.Length < 8;
		while (NotMatch() || NotAcceptableLength())
		{
			if (NotMatch()) { ColorMessage("Пароли не совпадают"); }
			if (NotAcceptableLength()) { ColorMessage("Пароль должен быть не менее 8 символов"); }
			(password01, password02) = DoublePassword();
		}
		return password01;
	}
}

