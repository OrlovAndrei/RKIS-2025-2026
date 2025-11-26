public class DeleteCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList Todos { get; set; }
	public string TodoFilePath { get; set; }
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
		FileManager.SaveTodos(Todos, TodoFilePath);
	}
	public void Unexecute()
	{
		if (deletedItem != null)
		{
			if (deletedIndex >= 0 && deletedIndex <= Todos.Count)
			{
				Todos.Add(deletedItem);
				Console.WriteLine("Удаление задачи отменено - задача восстановлена");
				FileManager.SaveTodos(Todos, TodoFilePath);
			}
		}
		else
		{
			Console.WriteLine("Невозможно отменить удаление - задача не найдена");
		}
	}
}
