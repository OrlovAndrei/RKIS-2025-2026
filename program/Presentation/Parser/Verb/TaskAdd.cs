using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "task-add", aliases: ["ta"])]
internal class TaskAdd
{
	[Option(longName: "state-id", shortName: 'S')]
	public int? StateId { get; set; }

	[Option(longName: "priority-level", shortName: 'P')]
	public int? PriorityLevel { get; set; }

	[Option(longName: "name", shortName: 'n')]
	public string? Name { get; set; }

	[Option(longName: "description", shortName: 'd')]
	public string? Description { get; set; }

	[Option(longName: "deadline", shortName: 'D')]
	public DateTime? Deadline { get; set; }
}
