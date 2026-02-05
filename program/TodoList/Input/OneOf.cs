using Spectre.Console;

namespace ShevricTodo.Input;

internal class OneOf
{
	public static string GetOneFromList(string? title = null, int pageSize = 3, params IEnumerable<string> options)
	{
		SelectionPrompt<string> selectionPrompt = new SelectionPrompt<string>()
			.PageSize(pageSize)
			.AddChoices(options);
		if (title is not null) { selectionPrompt.Title(title); }
		return AnsiConsole.Prompt(selectionPrompt);
	}
	public static async Task<KeyValuePair<int, string>> GetOneFromList(Dictionary<int, string> options, string? title = null, int pageSize = 3)
	{
		string[] value = options.Values.ToArray();
		string resString = GetOneFromList(options: value, pageSize: pageSize, title: title);
		return options.First(p => p.Value == resString);
	}x
}
