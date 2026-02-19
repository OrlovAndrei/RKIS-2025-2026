namespace Domain;

public class TaskState
{
	public Guid StateId { get; private set; }
	public string Name { get; private set; }
	public string? Description { get; private set; }
	public bool IsCompleted { get; private set; }

	public TaskState(
		string name,
		bool isCompleted,
		string? description = null)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Task status name cannot be null or empty.", nameof(name));
		}
		if (name.Length > 100)
		{
			throw new ArgumentException("Task status name cannot exceed 100 characters.", nameof(name));
		}
		if (description?.Length > 500)
		{
			throw new ArgumentException("Description cannot exceed 500 characters.", nameof(description));
		}
		StateId = Guid.NewGuid();
		Name = name;
		Description = description;
		IsCompleted = isCompleted;
	}
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
	private TaskState() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
	public static TaskState Restore(
		Guid stateId,
		string name,
		bool isCompleted,
		string? description) => new()
		{
			StateId = stateId,
			Name = name,
			Description = description,
			IsCompleted = isCompleted
		};
}