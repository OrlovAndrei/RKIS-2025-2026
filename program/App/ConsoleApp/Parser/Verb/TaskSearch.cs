using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "task-search", aliases: ["ts"])]
internal class TaskSearch
{
	[Option(longName: "task-id", shortName: 'i')]
	public Guid? TaskId { get; set; }

	[Option(longName: "state-id", shortName: 'S')]
	public int? StateId { get; set; }

	[Option(longName: "priority-level-from", shortName: 'p')]
	public int? PriorityLevelFrom { get; set; }

	[Option(longName: "priority-level-to", shortName: 'P')]
	public int? PriorityLevelTo { get; set; }

	[Option(longName: "name", shortName: 'n')]
	public string? Name { get; set; }

	[Option(longName: "description", shortName: 'd')]
	public string? Description { get; set; }

	[Option(longName: "created-at-from", shortName: 'c')]
	public DateTime? CreatedAtFrom { get; set; }

	[Option(longName: "created-at-to", shortName: 'C')]
	public DateTime? CreatedAtTo { get; set; }

	[Option(longName: "deadline-from", shortName: 'F')]
	public DateTime? DeadlineFrom { get; set; }

	[Option(longName: "deadline-to", shortName: 'T')]
	public DateTime? DeadlineTo { get; set; }

	[Option(longName: "search-type", shortName: 't')]
	public string? SearchType { get; set; }
}
