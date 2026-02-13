using System;
using ToDoList;

namespace TodoList.Commands
{
	public class AddCommand : ICommand, IUndo
	{
		public string TaskText { get; set; }
		public bool IsMultiline { get; set; }
		public bool IsUrgent { get; set; }

		private TodoItem addedItem;
		private int addedIndex;

		public void Execute()
		{
			if (string.IsNullOrEmpty(TaskText) && !IsMultiline)
			{
				Console.Write("Введите текст задачи: ");
				TaskText = Console.ReadLine();
			}

			string task = TaskText ?? "";

			if (IsUrgent)
			{
				task = task.Replace("-u", "").Trim();
				task = task.Trim('"');

				if (!task.StartsWith("[Срочно]"))
				{
					task = "[Срочно] " + task;
				}
			}

			if (IsMultiline)
			{
				Console.WriteLine("Введите задачу (для завершения введите пустую строку):");
				string multiTask = "";
				string line;
				while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
				{
					multiTask += line + "\n";
				}
				task = multiTask.TrimEnd();
			}

			if (string.IsNullOrWhiteSpace(task))
			{
				Console.WriteLine("Ошибка: текст задачи не может быть пустым");
				return;
			}

			addedItem = new TodoItem(task);
			AppInfo.Todos.Add(addedItem);
			addedIndex = AppInfo.Todos.Count - 1;

			Console.WriteLine($"Задача добавлена: {addedItem.Text}");
		}

		public void Unexecute()
		{
			if (addedItem != null)
			{
				AppInfo.Todos.Delete(addedIndex);
				Console.WriteLine($"Отменено добавление задачи: {addedItem.Text}");
			}
		}
	}
}