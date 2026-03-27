using System.Text;

namespace Todolist;

public class AddCommand : ICommand
{
	public string TaskText { get; set; }
	public bool MultilineMode { get; set; }
	public string TodoFilePath { get; set; }
	public string Description => $"Добавление задачи: {TaskText}";

	private TodoItem _createdTask;

	public void Execute()
	{
		if (MultilineMode)
		{
			AddTodoMultiline();
		}
		else
		{
			if (string.IsNullOrWhiteSpace(TaskText))
			{
				Console.WriteLine("Ошибка: задача не может быть пустой");
				return;
			}
			_createdTask = new TodoItem(TaskText);
			AppInfo.Todos.Add(_createdTask);
			Console.WriteLine("Задача добавлена");
		}

		FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
	}

	public void Unexecute()
	{
		if (_createdTask != null)
		{
			AppInfo.Todos.Remove(_createdTask);  // ← удаляем из списка
			Console.WriteLine($"Отмена: задача '{_createdTask.Text}' удалена");

			// Сохраняем изменения
			FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
		}
	}

	private void AddTodoMultiline()
	{
		Console.WriteLine("Введите задачу построчно. Для завершения введите '!end':");
		List<string> lines = new();
		int max = 100;

		while (true)
		{
			Console.Write("> ");
			string line = Console.ReadLine();
			if (string.IsNullOrEmpty(line)) continue;

			if (line == "!end") break;

			if (lines.Count() >= max)
			{
				Console.WriteLine("Достигнут лимит строк (100). Завершите ввод");
				break;
			}
			lines.Add(line);
		}

		if (lines.Count() == 0)
		{
			Console.WriteLine("Задача не была добавлена - пустой ввод");
			return;
		}
		StringBuilder taskhui = new();
		taskhui.Append(string.Join(" | ", lines));
		TodoItem newItem = new TodoItem(taskhui.ToString());
		AppInfo.Todos.Add(newItem);
		Console.WriteLine("Многострочная задача добавлена");
	}
}