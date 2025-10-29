public class DeleteCommand : ICommand
{
	public int TaskIndex {get; set;}
	public TodoList Todos {get; set;}
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		Todos.Delete(TaskIndex);
		Console.WriteLine($"Задача {TaskIndex} удалена");
	}
}
