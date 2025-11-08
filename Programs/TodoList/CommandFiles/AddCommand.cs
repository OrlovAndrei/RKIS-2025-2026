using System;

namespace Todolist
{
	public class AddCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public string TaskText { get; set; }
		public bool MultilineMode { get; set; }

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
		}

		private void AddTodoMultiline()
		{
			Console.WriteLine("Введите задачу построчно. Для завершения введите '!end':");
			string[] lines = new string[100];
			int lineCount = 0;

			while (true)
			{
				Console.Write("> ");
				string line = Console.ReadLine();
				if (string.IsNullOrEmpty(line)) continue;

				if (line == "!end") break;

				if (lineCount >= lines.Length)
				{
					Console.WriteLine("Достигнут лимит строк (100). Завершите ввод");
					break;
				}
				lines[lineCount] = line;
				lineCount++;
			}

			if (lineCount == 0)
			{
				Console.WriteLine("Задача не была добавлена - пустой ввод");
				return;
			}

			string task = "";
			for (int i = 0; i < lineCount; i++)
			{
				task += lines[i];
				if (i < lineCount - 1)
				{
					task += "\n";
				}
			}
			TodoItem newItem = new TodoItem(task);
			TodoList.Add(newItem);
			Console.WriteLine("Многострочная задача добавлена");
		}
	}
}