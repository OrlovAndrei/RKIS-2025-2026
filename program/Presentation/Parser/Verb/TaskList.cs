using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "task-list", aliases: ["tl"])]
internal class TaskList
{
	[Option(longName: "top", shortName: 't')]
	public int? Top { get; set; }
}
