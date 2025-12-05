namespace TodoList.Commands
{
	public class ReadCommand : ICommand
	{
		public string Arg { get; set; }

		public void Execute()
		{
			var todos = AppInfo.CurrentUserTodos;
			if (todos == null)
			{
				Console.WriteLine("Ошибка: не удалось получить список задач. Войдите в профиль.");
				return;
			}

			todos.ReadTask(Arg);
		}

		public void Unexecute() { }
	}
}