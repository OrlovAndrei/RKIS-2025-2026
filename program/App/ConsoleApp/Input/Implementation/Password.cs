using ConsoleApp.Output.Interfaces;
using Spectre.Console;

namespace ConsoleApp.Input.Implementation;

internal class Password(IColoredOutput coloredOutput)
{
	private readonly IColoredOutput _coloredOutput = coloredOutput;
	public static string GetPassword(string message)
	{
		var password = AnsiConsole.Prompt(
			new TextPrompt<string>(message)
				.Secret());
		return password;
	}
	private static (string password01, string password02) DoublePassword() => (GetPassword("Введите пароль: "), GetPassword("Повторите пароль пароль: "));

	public string CheckingThePassword()
	{
		string password01 = string.Empty;
		string password02 = string.Empty;
		(password01, password02) = DoublePassword();
		while (NotMatch() || NotAcceptableLength())
		{
			if (NotMatch()) { _coloredOutput.WriteColoredLine("Пароли не совпадают", ConsoleColor.Red); }
			if (NotAcceptableLength()) { _coloredOutput.WriteColoredLine("Пароль должен быть не менее 8 символов", ConsoleColor.Red); }
			(password01, password02) = DoublePassword();
		}
		return password01;
		bool NotMatch() => password01 != password02;
		bool NotAcceptableLength() => password01.Length < 8;
	}
}
