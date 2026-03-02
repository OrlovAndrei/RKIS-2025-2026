using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "task-remove", aliases: ["tr"])]
internal class TaskRemove
{
	[Option(longName: "task-id", shortName: 'i')]
	public Guid? TaskId { get; set; }

	[Option(longName: "name", shortName: 'n')]
	public string? Name { get; set; }

	[Option(longName: "description", shortName: 'd')]
	public string? Description { get; set; }

	[Option(longName: "deadline-from", shortName: 'F')]
	public DateTime? DeadlineFrom { get; set; }

	[Option(longName: "deadline-to", shortName: 'T')]
	public DateTime? DeadlineTo { get; set; }
}
