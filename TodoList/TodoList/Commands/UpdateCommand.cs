public class UpdateCommand : ICommand
{
	public int TaskIndex { get; set; }
	public string NewText { get; set; }
	public TodoList Todos { get; set; }
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		Todos[TaskIndex].UpdateText(NewText);
		Console.WriteLine($"Задача {TaskIndex} обновлена!");
	}
	public void Unexecute()
	{
		Console.WriteLine("Отмена обновления пока не реализована");
	}
}