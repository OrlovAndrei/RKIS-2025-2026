namespace TodoList.Commands
{
	public class StatusCommand : ICommand
	{
		public string Arg { get; set; }

		private TodoStatus _originalStatus;
		private int _statusIndex;

		public void Execute()
		{
			var todos = AppInfo.CurrentUserTodos;
			if (todos == null)
			{
				Console.WriteLine("Ошибка: не удалось получить список задач. Войдите в профиль.");
				return;
			}

			var parts = Arg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи и статус");
				return;
			}

			_statusIndex = idx - 1;
			if (_statusIndex < 0 || _statusIndex >= todos.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_originalStatus = todos.tasks[_statusIndex].Status;
			if (Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
			{
				todos.tasks[_statusIndex].SetStatus(status);
				Console.WriteLine($"Статус задачи {idx} изменен на: {status}");
				Program.SaveCurrentUserTasks();
			}
			else
			{
				Console.WriteLine("Ошибка: некорректный статус. Допустимые значения: NotStarted, InProgress, Completed, Postponed, Failed");
			}
		}

		public void Unexecute()
		{
			var todos = AppInfo.CurrentUserTodos;
			if (todos != null)
			{
				todos.tasks[_statusIndex].SetStatus(_originalStatus);
				Console.WriteLine("Изменение статуса отменено");
				Program.SaveCurrentUserTasks();
			}
		}
	}
}