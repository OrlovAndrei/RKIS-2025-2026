using System;

namespace Todolist
{
	public class StatusCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskNumber { get; set; }
		public TodoStatus NewStatus { get; set; }
		public string TodoFilePath { get; set; }

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= TodoList.Count)
			{
				int index = TaskNumber - 1;
				TodoList.SetStatus(index, NewStatus);

				string statusText = NewStatus switch
				{
					TodoStatus.NotStarted => "Не начато",
					TodoStatus.InProgress => "В процессе",
					TodoStatus.Completed => "Выполнено",
					TodoStatus.Postponed => "Отложено",
					TodoStatus.Failed => "Провалено",
					_ => NewStatus.ToString()
				};

				Console.WriteLine($"Статус задачи '{TaskNumber}' изменён на '{statusText}'");

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