using Spectre.Console;

namespace ShevricTodo.Input;

internal class OneOf
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
	public static async Task<IDictionary<int, string>> GetOneFromList(Dictionary<int, string> option)
	{
		IDictionary<int, string> res = await AnsiConsole.PromptAsync(
			new SelectionPrompt<IDictionary<int, string>>()
				.Title("Выберите один из [green]вариантов[/]:")
				.PageSize(5)
				// .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
				.AddChoices(option));
		return res;
	}
}
