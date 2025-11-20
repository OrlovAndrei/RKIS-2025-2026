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

		var task = Todos[TaskIndex];
		task.SetStatus(NewStatus);
	}
}
