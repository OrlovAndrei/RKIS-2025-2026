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
	/// <summary>
	/// Asynchronously retrieves a single key-value pair from the specified dictionary, optionally filtering by title and
	/// limiting the number of items considered.
	/// </summary>
	/// <remarks>If the title parameter is not specified, the method selects a value from the first pageSize items
	/// in the dictionary. The selection is performed asynchronously.</remarks>
	/// <param name="options">A dictionary containing integer keys and string values from which to select a key-value pair.</param>
	/// <param name="title">An optional string used to filter the dictionary values by title. If null, no filtering is applied.</param>
	/// <param name="pageSize">The maximum number of items to consider when selecting a value from the dictionary. The default is 3.</param>
	/// <returns>A key-value pair representing the selected item from the dictionary.</returns>
	public static async Task<KeyValuePair<int, string>> GetOneFromList(Dictionary<int, string> options, string? title = null, int pageSize = 3)
	{
		string[] value = options.Values.ToArray();
		string resString = GetOneFromList(options: value, pageSize: pageSize, title: title);
		return options.First(p => p.Value == resString);
	}
}
