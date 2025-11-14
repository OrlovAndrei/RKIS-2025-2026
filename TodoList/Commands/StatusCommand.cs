namespace TodoList.Commands
{
	public class StatusCommand : ICommand
	{
		public string Arg { get; set; }
		public TodoList TodoList { get; set; }

		public void Execute()
		{
			if (TodoList == null)
			{
				Console.WriteLine("Ошибка: список задач не инициализирован.");
				return;
			}

			var parts = Arg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи и статус");
				return;
			}

			idx--;
			if (idx < 0 || idx >= TodoList.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			if (Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
			{
				TodoList.tasks[idx].SetStatus(status);
				Console.WriteLine($"Статус задачи {idx + 1} изменен на: {status}");
				FileManager.SaveTodos(TodoList, "data/todo.csv");
			}
			else
			{
				Console.WriteLine("Ошибка: некорректный статус. Допустимые значения: NotStarted, InProgress, Completed, Postponed, Failed");
			}
		}
	}
}