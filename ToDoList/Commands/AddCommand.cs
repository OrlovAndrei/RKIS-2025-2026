using System;

namespace TodoList
{
	class AddCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public bool IsMultiline { get; set; }
		public bool IsUrgent { get; set; }
		public string TaskText { get; set; }

		public TodoItem addedItem;
		public int addedIndex;

		public void Execute()
		{
			string task = TaskText;
			if (IsMultiline)
			{
				Console.WriteLine("Введите задачу (для завершения введите пустую строку):");
				task = "";
				string line;
				while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
				{
					task += line + "/n";
				}
				task = task.TrimEnd();
			}

			if (string.IsNullOrEmpty(task))
			{
				Console.WriteLine("Ошибка: текст задачи не может быть пустым");
				return;
			}

			if (IsUrgent)
			{
				task = "[Срочно] " + task;
			}
			addedItem = new TodoItem(task);
			TodoList.Add(addedItem);
			addedIndex = TodoList.Count - 1;
			Console.WriteLine($"Задача добавлена: {task}");
		}

		public void Unexecute()
		{
			if (addedItem != null && TodoList.Count > 0)
			{
				TodoList.Delete(addedIndex);
				Console.WriteLine($"Отменено добавление задачи: {addedItem.Text}");
			}
		}
	}
}
