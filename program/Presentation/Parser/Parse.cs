using CommandLine;
using Presentation.Output.Implementation;
using System.Reflection;

namespace Presentation.Parser;

internal static class Parse
{
	public static void Run(string[] args)
	{
		try
		{
			var types = LoadVerbs();
			CommandLine.Parser.Default.ParseArguments(args, types)
			.WithParsed(RunOptions.Run);
			//.WithNotParsed();
		}
		catch (Exception ex)
		{
			WriteToConsole.ProcExcept(ex);
		}
	}
	private static Type[] LoadVerbs() => Assembly.GetExecutingAssembly().GetTypes()
			.Where(t => t.GetCustomAttribute<VerbAttribute>() != null && !t.IsNested).ToArray();

	public static DateTime? ParseDate(string? parsedDate)
	{
		if (parsedDate is not null)
		{
			DateTime readyDate;
			if (!DateTime.TryParse(parsedDate, out readyDate))
			{
				throw new Exception(message: "Неправильный формат даты!");
			}
			return readyDate;
		}
		return null;
	}
}
