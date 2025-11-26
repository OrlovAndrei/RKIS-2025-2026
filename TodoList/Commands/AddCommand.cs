using TodoApp;
namespace TodoApp.Commands;
public class AddCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public string TaskText { get; set; }
	public bool Multiline { get; set; }
	private List<TodoItem> _addedItems = new List<TodoItem>();
	private List<int> _addedIndexes = new List<int>();

	public AddCommand()
	{
		TodoList = AppInfo.Todos;
	}

	public AddCommand(string taskText, bool multiline = false) : this()
	{
		TaskText = taskText;
		Multiline = multiline;
	}

	public override void Execute()
	{
		_addedItems.Clear();
		_addedIndexes.Clear();

		if (Multiline)
		{
			AddMultilineTask();
		}
		else
		{
			if (!string.IsNullOrEmpty(TaskText))
			{
				var item = new TodoItem(TaskText);
				TodoList.Add(item);
				_addedItems.Add(item);
				_addedIndexes.Add(TodoList.Count - 1);
				Console.WriteLine("Задача добавлена.");
			}
			else
			{
				Console.WriteLine("Ошибка: задача не может быть пустой");
			}
		}
	}

	public override void Unexecute()
	{
		for (int i = _addedIndexes.Count - 1; i >= 0; i--)
		{
			if (_addedIndexes[i] < TodoList.Count)
			{
				TodoList.Delete(_addedIndexes[i]);
			}
		}
		Console.WriteLine($"Отменено добавление {_addedItems.Count} задач(и)");
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
				var item = new TodoItem(finalTask);
				TodoList.Add(item);
				_addedItems.Add(item);
				_addedIndexes.Add(TodoList.Count - 1);
			}
		}
		Console.WriteLine($"Добавлено {lines.Count} задач(и)");
	}
}
