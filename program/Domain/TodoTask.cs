namespace Domain;

public class TodoTask
{
	public Guid TaskId { get; private set; }
	public Guid StateId { get; private set; }
	public Guid ProfileId { get; private set; }
	public string Name { get; private set; }
	public string? Description { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public DateTime? Deadline { get; private set; }
	public TodoTask(
		Guid statusId,
		Guid profileId,
		string name,
		string? description = null,
		DateTime? deadline = null)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Task name cannot be null or empty.", nameof(name));
		}
		if (name.Length > 100)
		{
			throw new ArgumentException("Task name cannot exceed 100 characters.", nameof(name));
		}
		if (deadline < DateTime.UtcNow)
		{
			throw new ArgumentException("Deadline cannot be in the past.", nameof(deadline));
		}
		if (description?.Length > 500)
		{
			throw new ArgumentException("Description cannot exceed 500 characters.", nameof(description));
		}
		if (statusId == Guid.Empty || statusId == default)
		{
			throw new ArgumentException("Status ID cannot be empty.", nameof(statusId));
		}
		if (profileId == Guid.Empty || profileId == default)
		{
			throw new ArgumentException("Profile ID cannot be empty.", nameof(profileId));
		}
		TaskId = Guid.NewGuid();
		StateId = statusId;
		ProfileId = profileId;
		Name = name;
		Description = description;
		CreatedAt = DateTime.UtcNow;
		Deadline = deadline;
	}
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
	private TodoTask() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
	public static TodoTask Restore(
		Guid taskId,
		Guid stateId,
		Guid profileId,
		string name,
		string? description,
		DateTime createdAt,
		DateTime? deadline) => new()
		{
			TaskId = taskId,
			StateId = stateId,
			ProfileId = profileId,
			Name = name,
			Description = description,
			CreatedAt = createdAt,
			Deadline = deadline
		};
}
