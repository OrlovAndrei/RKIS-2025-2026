using TodoApp;
namespace TodoApp.Commands;
public class AddCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public string TaskText { get; set; }
	public bool Multiline { get; set; }

	public override void Execute()
	{
		if (Multiline)
		{
			AddMultilineTask();
		}
		else
		{
			if (!string.IsNullOrEmpty(TaskText))
			{
				TodoList.Add(new TodoItem(TaskText));
				Console.WriteLine("Задача добавлена.");
			}
			else
			{
				Console.WriteLine("Ошибка: задача не может быть пустой");
			}
		}
	}
	private void AddMultilineTask()
	{
		Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите !end):");
		var lines = new List<string>();
		string line;
		while (true)
		{
			Console.Write("> ");
			line = Console.ReadLine();
			if (line == "!end") break;
			if (!string.IsNullOrWhiteSpace(line))
			{
				lines.Add(line);
			}
		}
		foreach (string finalTask in lines)
		{
			if (!string.IsNullOrEmpty(finalTask))
			{
				TodoList.Add(new TodoItem(finalTask));
			}
		}
		Console.WriteLine($"Добавлено {lines.Count} задач(и)");
	}
}

