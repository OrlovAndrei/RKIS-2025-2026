namespace Domain.Entities.TaskEntity;

public class TaskPriority
{
	public int Level { get; }
	public string Name { get; }
	private TaskPriority(
		int level,
		string name
	)
	{
		Level = level;
		Name = name;
	}
	public static readonly TaskPriority Low = new(level: 1, name: "Low");
	public static readonly TaskPriority Medium = new(level: 2, name: "Medium");
	public static readonly TaskPriority High = new(level: 3, name: "High");
	public static readonly TaskPriority Critical = new(level: 4, name: "Critical");
	public TaskPriority Escalate()
	{
		return this switch
		{
			_ when this == Low => Medium,
			_ when this == Medium => High,
			_ when this == High => Critical,
			_ when this == Critical => Critical,
			_ => this
		};
	}
	public TaskPriority Deescalate()
	{
		return this switch
		{
			_ when this == Low => Low,
			_ when this == Medium => Low,
			_ when this == High => Medium,
			_ when this == Critical => High,
			_ => this
		};
	}
	public static class ListPriority
	{
		private readonly static TaskPriority[] taskPriorities = [Low, Medium, High, Critical];
		public static TaskPriority GetByLevel(int level) => taskPriorities.FirstOrDefault(p => p.Level == level, Medium);
	}
}