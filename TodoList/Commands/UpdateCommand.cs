namespace TodoList.Commands
{
	public class UpdateCommand : ICommand
	{
		public string Arg { get; set; }

		private string _originalText;
		private int _updatedIndex;

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
				Console.WriteLine("Ошибка: укажите номер задачи и текст");
				return;
			}

			_updatedIndex = idx - 1;
			if (_updatedIndex < 0 || _updatedIndex >= todos.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_originalText = todos.tasks[_updatedIndex].Text;
			todos.tasks[_updatedIndex].UpdateText(parts[1].Trim('"', '\''));
			Console.WriteLine("Задача обновлена");
			Program.SaveCurrentUserTasks();
		}

		public void Unexecute()
		{
			var todos = AppInfo.CurrentUserTodos;
			if (_originalText != null && todos != null)
			{
				todos.tasks[_updatedIndex].UpdateText(_originalText);
				Console.WriteLine("Обновление задачи отменено");
				Program.SaveCurrentUserTasks();
			}
		}
	}
}