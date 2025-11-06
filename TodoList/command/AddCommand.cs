namespace TodoApp.Commands;

public class AddCommand : ICommand
{
	public string Name => "add";
	public string Description => "Добавить новую задачу";

	// Флаги команд как свойства bool
	public bool Multiline { get; set; }

	// Введённый текст
	public string TaskText { get; set; }

	// Свойства для работы с данными
	public TodoList TodoList { get; set; }

	public bool Execute()
	{
		if (TodoList == null)
		{
			Console.WriteLine(" Ошибка: TodoList не установлен");
			return false;
		}

		if (Multiline)
		{
			return AddMultilineTask();
		}
		else
		{
			return AddSingleLineTask();
		}
	}

	private bool AddSingleLineTask()
	{
		string taskText = TaskText;

		if (string.IsNullOrWhiteSpace(taskText))
		{
			Console.Write("Введите текст задачи: ");
			taskText = Console.ReadLine()?.Trim();
		}

		if (!string.IsNullOrWhiteSpace(taskText))
		{
			taskText = taskText.StartsWith("\"") && taskText.EndsWith("\"")
				? taskText.Substring(1, taskText.Length - 2)
				: taskText;

			TodoItem newTask = new TodoItem(taskText);
			TodoList.Add(newTask);
			Console.WriteLine(" Задача успешно добавлена!");
			return true;
		}
		else
		{
			Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
			return false;
		}
	}

	private bool AddMultilineTask()
	{
		Console.WriteLine("Введите текст задачи (для завершения введите !end):");
		System.Text.StringBuilder taskText = new System.Text.StringBuilder();
		string line;

		while (true)
		{
			Console.Write("> ");
			line = Console.ReadLine()?.Trim() ?? "";

			if (line == "!end")
				break;

			if (taskText.Length > 0)
				taskText.AppendLine();

			taskText.Append(line);
		}

		string finalText = taskText.ToString();
		if (!string.IsNullOrWhiteSpace(finalText))
		{
			TodoItem newTask = new TodoItem(finalText);
			TodoList.Add(newTask);
			Console.WriteLine(" Задача успешно добавлена!");
			return true;
		}
		else
		{
			Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
			return false;
		}
	}
}