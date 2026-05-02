using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "task-list", aliases: ["tl"])]
internal class TaskList
{
	[Option(longName: "top", shortName: 't')]
	public int? Top { get; set; }
}
