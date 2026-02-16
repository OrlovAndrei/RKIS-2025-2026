using CommandLine;
using System.Reflection;

namespace ShevricTodo.Parser;

internal static class Parse
{
	public static void Run(string[] args)
	{
		var types = LoadVerbs();
		CommandLine.Parser.Default.ParseArguments(args, types)
		.WithParsed(RunOptions.Run);
		//.WithNotParsed();
	}
	private static Type[] LoadVerbs() => Assembly.GetExecutingAssembly().GetTypes()
			.Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();

	public static DateTime? ParseDate(string? parsedDate)
	{
		DateTime? readyDate = null;
		if (parsedDate != null)
		{
			try
			{
				readyDate = DateTime.Parse(parsedDate);
			}
			catch (FormatException)
			{
				Input.WriteToConsole.ColorMessage("Неправильный формат даты!");
			}
		}
		return readyDate;
	}
}
