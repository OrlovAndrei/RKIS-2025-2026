public class MarkDoneCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList Todos { get; set; }
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		Todos.GetItem(TaskIndex).SetStatus(TodoStatus.Completed);
		Console.WriteLine($"Задача {TaskIndex} отмечена как выполненная!");
	}
}