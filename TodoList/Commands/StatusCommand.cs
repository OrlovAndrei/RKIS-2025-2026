namespace TodoList.Commands
{
	public class StatusCommand : ICommand
	{
		public string Arg { get; set; }
		public TodoList TodoList { get; set; }

		private TodoStatus _originalStatus;
		private int _statusIndex;

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

			_statusIndex = idx - 1;
			if (_statusIndex < 0 || _statusIndex >= TodoList.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_originalStatus = TodoList.tasks[_statusIndex].Status;
			if (Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
			{
				TodoList.tasks[_statusIndex].SetStatus(status);
				Console.WriteLine($"Статус задачи {idx} изменен на: {status}");
				FileManager.SaveTodos(TodoList, "data/todo.csv");
			}
			else
			{
				Console.WriteLine("Ошибка: некорректный статус. Допустимые значения: NotStarted, InProgress, Completed, Postponed, Failed");
			}
		}

		public void Unexecute()
		{
			if (TodoList != null)
			{
				TodoList.tasks[_statusIndex].SetStatus(_originalStatus);
				Console.WriteLine("Изменение статуса отменено");
				FileManager.SaveTodos(TodoList, "data/todo.csv");
			}
		}
	}
}