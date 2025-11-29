public class UpdateCommand : ICommand
{
	public int TaskIndex { get; set; }
	public string NewText { get; set; }
	public TodoList Todos { get; set; }
	public string TodoFilePath { get; set; }
	private string oldText;
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		oldText = Todos.GetItem(TaskIndex).GetText();
		Todos.GetItem(TaskIndex).UpdateText(NewText);
		Console.WriteLine($"Задача {TaskIndex} обновлена!");
		FileManager.SaveTodos(Todos, TodoFilePath);
	}
	public void Unexecute()
	{
		if (!string.IsNullOrEmpty(oldText))
		{
			Todos.GetItem(TaskIndex).UpdateText(oldText);
			Console.WriteLine("Обновление задачи отменено");
			FileManager.SaveTodos(Todos, TodoFilePath);
		}
	}
}