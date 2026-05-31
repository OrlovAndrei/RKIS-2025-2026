namespace Domain.Entities.TaskEntity;

public class TaskState
{
	public int StateId { get; }
	public string Name { get; }
	public string Description { get; }
	public CompletionIndex CompletionIndex { get; }
	private TaskState(
		ushort stateId,
		string name,
		string description,
		CompletionIndex completionIndex)
	{
		StateId = stateId;
		Name = name;
		Description = description;
		CompletionIndex = completionIndex;
	}
	public static readonly TaskState Uncertain = new(
		stateId: 1,
		name: "Uncertain",
		description: "The task status has not yet been determined.",
		completionIndex: CompletionIndex.Default
	);
	public static readonly TaskState Completed = new(
		stateId: 2,
		name: "Completed",
		description: "Task completed.",
		completionIndex: CompletionIndex.Max
	);
	public static readonly TaskState InProgress = new(
		stateId: 3,
		name: "In progress",
		description: "The task is in the process of being completed.",
		completionIndex: CompletionIndex.Max / 3
	);
	public static readonly TaskState NotCompleted = new(
		stateId: 4,
		name: "Not completed",
		description: "The task was not completed.",
		completionIndex: CompletionIndex.Min
	);

	public static class ListState
	{
		private readonly static TaskState[] taskStates = [Uncertain, Completed, InProgress, NotCompleted];
		public static TaskState GetById(int stateId) => taskStates.FirstOrDefault(s => s.StateId == stateId, Uncertain);
	}
}