namespace Todolist;

public class ReadCommand : ICommand
{
	public int TaskNumber { get; set; }
	public string Description => $"Просмотр задачи #{TaskNumber}";

	public void Execute()
	{
		if (TaskNumber > 0 && TaskNumber <= AppInfo.Todos.Count)
		{
			int index = TaskNumber - 1;
			TodoItem item = AppInfo.Todos.GetItem(index);
			Console.WriteLine("=======================================");
			Console.WriteLine(item.GetFullInfo());
			Console.WriteLine("=======================================");
		}
		else
		{
			Console.WriteLine("Неверный номер задачи");
		}
	}

	public void Unexecute() { } // Круто, можно unread чисто, это типа просто забыть что прочитал
}