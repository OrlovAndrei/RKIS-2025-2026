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
	private static Type[] LoadVerbs()
	{
		return Assembly.GetExecutingAssembly().GetTypes()
			.Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
	}
}
