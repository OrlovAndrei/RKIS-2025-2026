namespace TodoList.Commands
{
	public class DeleteCommand : ICommand
	{
		public string Arg { get; set; }
		public TodoList TodoList { get; set; }

		private TodoItem _deletedItem;
		private int _deletedIndex;

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

			_deletedIndex = idx - 1;
			if (_deletedIndex < 0 || _deletedIndex >= TodoList.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_deletedItem = TodoList.tasks[_deletedIndex];
			TodoList.tasks.RemoveAt(_deletedIndex);
			Console.WriteLine("Задача удалена");
			FileManager.SaveTodos(TodoList, "data/todo.csv");
		}

		public void Unexecute()
		{
			if (_deletedItem != null && TodoList != null)
			{
				TodoList.tasks.Insert(_deletedIndex, _deletedItem);
				Console.WriteLine("Удаление задачи отменено");
				FileManager.SaveTodos(TodoList, "data/todo.csv");
			}
		}
	}
}