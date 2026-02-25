using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "task-edit", aliases: ["te"])]
internal class TaskEdit
{
	#region search
	[Option(longName: "search-task-id", shortName: 'i')]
	public Guid? TaskIdSearch { get; set; }

	[Option(longName: "search-state-id", shortName: 'S')]
	public int? StateIdSearch { get; set; }

	[Option(longName: "search-priority-level-from", shortName: 'p')]
	public int? PriorityLevelFromSearch { get; set; }

	[Option(longName: "search-priority-level-to", shortName: 'P')]
	public int? PriorityLevelToSearch { get; set; }

	[Option(longName: "search-name", shortName: 'n')]
	public string? NameSearch { get; set; }

	[Option(longName: "search-description", shortName: 'd')]
	public string? DescriptionSearch { get; set; }

	[Option(longName: "search-created-at-from", shortName: 'c')]
	public DateTime? CreatedAtFromSearch { get; set; }

	[Option(longName: "search-created-at-to", shortName: 'C')]
	public DateTime? CreatedAtToSearch { get; set; }

	[Option(longName: "search-deadline-from", shortName: 'F')]
	public DateTime? DeadlineFromSearch { get; set; }

	[Option(longName: "search-deadline-to", shortName: 'T')]
	public DateTime? DeadlineToSearch { get; set; }
	#endregion

	#region execute
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
	#endregion
}
