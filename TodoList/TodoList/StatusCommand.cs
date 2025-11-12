public class StatusCommand : ICommand
{
	public TodoList Todos { get; set; }
	public int TaskIndex { get; set; }
	public TodoStatus NewStatus { get; set; }
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}

		var task = Todos.GetItem(TaskIndex);
		task.SetStatus(NewStatus);
		Console.WriteLine($"Статус задачи {TaskIndex} изменен на: {GetStatusText(NewStatus)}");
	}
	private string GetStatusText(TodoStatus status)
	{
		return status switch
		{
			TodoStatus.NotStarted => "Не начато",
			TodoStatus.InProgress => "В процессе",
			TodoStatus.Completed => "Выполнено",
			TodoStatus.Postponed => "Отложено",
			TodoStatus.Failed => "Провалено",
			_ => "Неизвестно"
		};
	}
}
