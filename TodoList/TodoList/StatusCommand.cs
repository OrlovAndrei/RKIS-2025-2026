public class StatusCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoStatus NewStatus { get; set; }
	public TodoList Todos { get; set; }
	public string TodoFilePath { get; set; }
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
	public void Unexecute()
	{
		Console.WriteLine("Отмена изменения статуса пока не реализована");
	}
}
