namespace TodoList.Commands
{
	public class DoneCommand : ICommand
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

			if (!int.TryParse(Arg, out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи");
				return;
			}

			idx--;
			if (idx < 0 || idx >= TodoList.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			TodoList.tasks[idx].SetStatus(TodoStatus.Completed);
			Console.WriteLine("Задача выполнена");
			FileManager.SaveTodos(TodoList, "data/todo.csv");
		}
	}
}