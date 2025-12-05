namespace TodoList.Commands
{
	public class ViewCommand : ICommand
	{
		public string[] Flags { get; set; } = Array.Empty<string>();

		public void Execute()
		{
			var todos = AppInfo.CurrentUserTodos;
			if (todos == null)
			{
				Console.WriteLine("Ошибка: не удалось получить список задач. Войдите в профиль.");
				return;
			}

			todos.ViewTasks(Flags);
		}

		public void Unexecute() { }
	}
}