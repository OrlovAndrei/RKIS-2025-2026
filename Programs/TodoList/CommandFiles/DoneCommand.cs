using System;

namespace Todolist
{
	public class DoneCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskNumber { get; set; }
		public string TodoFilePath { get; set; }

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= TodoList.Count)
			{
				int index = TaskNumber - 1;
				TodoItem item = TodoList.GetItem(index);
				item.MarkDone();
				Console.WriteLine($"Задача '{item.Text}' выполнена");

				// Сохраняем задачи после изменения
				if (!string.IsNullOrEmpty(TodoFilePath))
				{
					FileManager.SaveTodos(TodoList, TodoFilePath);
				}
			}
			else
			{
				Console.WriteLine("Неверный номер задачи");
			}
		}
	}
}