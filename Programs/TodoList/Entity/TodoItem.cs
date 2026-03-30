using TodoList.Interfaces;

namespace TodoList.Entity;

public class TodoItem
{
	public uint Id { get; init; }
	public Guid ProfileId { get; init; }
	public string Text { get; private set; }
	public TodoStatus Status { get; private set; } = TodoStatus.NotStarted;
	public DateTime LastUpdate { get; private set; }
	private readonly IClock _clock;
	public Profile? Profile { get; private set; }

	public TodoItem(
		string text,
		Guid profileId,
		IClock clock)
	{
		ProfileId = profileId;
		Text = text;
		_clock = clock;
		LastUpdate = _clock.Now();
	}
	public TodoItem(
		Guid profileId,
		string text,
		IClock clock,
		TodoStatus todoStatus = TodoStatus.NotStarted) : this(
			profileId: profileId,
			text: text,
			clock: clock
		)
	{
		Status = todoStatus;
	}
#pragma warning disable CS9264, CS8618
	public TodoItem() { }
#pragma warning restore CS9264, CS8618

	public void UpdateStatus(TodoStatus newStatus)
	{
		Status = newStatus;
		LastUpdate = _clock.Now();
	}

	public void UpdateText(string newText)
	{
		Text = newText;
		LastUpdate = _clock.Now();
	}
}