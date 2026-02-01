using System;
using System.Text;

namespace Todolist
{
	public class AddCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public string TaskText { get; set; }
		public bool MultilineMode { get; set; }
		public string TodoFilePath { get; set; }

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
				TodoItem newItem = new TodoItem(TaskText);
				TodoList.Add(newItem);
				Console.WriteLine("Задача добавлена");
			}

			// Сохраняем задачи после добавления
			if (!string.IsNullOrEmpty(TodoFilePath))
			{
				FileManager.SaveTodos(TodoList, TodoFilePath);
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
			TodoList.Add(newItem);
			Console.WriteLine("Многострочная задача добавлена");
		}
	}
}