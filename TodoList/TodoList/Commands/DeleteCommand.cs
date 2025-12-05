public class DeleteCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList Todos { get; set; }
	public string DataDir { get; set; }
	public Guid UserId { get; set; }
	private TodoItem deletedItem;
	private int deletedIndex;
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		deletedItem = Todos.GetItem(TaskIndex);
		deletedIndex = TaskIndex;
		Todos.Delete(TaskIndex);
		Console.WriteLine($"Задача {TaskIndex} удалена");
		FileManager.SaveUserTodos(UserId, Todos, DataDir);
	}
	public void Unexecute()
	{
		if (deletedItem != null)
		{
			Todos.Add(deletedItem);
			Console.WriteLine("Удаление задачи отменено");
			FileManager.SaveUserTodos(UserId, Todos, DataDir);
		}
	}
}
