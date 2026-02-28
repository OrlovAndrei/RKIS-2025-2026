using System;

namespace Todolist
{
	public class UpdateCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskNumber { get; set; }
		public string NewText { get; set; }
		public string TodoFilePath { get; set; }

		public void Execute()
		{
			if (string.IsNullOrWhiteSpace(NewText))
			{
				Console.WriteLine("Ошибка: новый текст не может быть пустым");
				return;
			}

			if (TaskNumber > 0 && TaskNumber <= TodoList.Count)
			{
				int index = TaskNumber - 1;
				TodoItem item = TodoList.GetItem(index);
				string oldTask = item.Text;
				item.UpdateText(NewText);
				Console.WriteLine($"Задача '{oldTask}' обновлена на '{NewText}'");

				// Сохраняем задачи после обновления
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