public class StatusCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoStatus NewStatus { get; set; }
	public TodoList Todos { get; set; }
	public string DataDir { get; set; }
	public Guid UserId { get; set; }
	private TodoStatus oldStatus;
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}

		oldStatus = Todos.GetItem(TaskIndex).GetStatus();
		Todos.GetItem(TaskIndex).UpdateStatus(NewStatus);
		Console.WriteLine($"Статус задачи {TaskIndex} изменен на: {Todos.GetItem(TaskIndex).GetStatusText()}");
		FileManager.SaveUserTodos(UserId, Todos, DataDir);
	}
	public void Unexecute()
	{
		if (TaskIndex >= 0 && TaskIndex < Todos.Count)
		{
			Todos.GetItem(TaskIndex).UpdateStatus(oldStatus);
			Console.WriteLine("Изменение статуса отменено");
			FileManager.SaveUserTodos(UserId, Todos, DataDir);
		}
	}
}
