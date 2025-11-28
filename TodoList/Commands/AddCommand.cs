namespace TodoList.Commands
{
	public class AddCommand : ICommand
	{
		public string Text { get; set; }
		public bool IsMultiline { get; set; }
		public string[] Flags { get; set; }
		public TodoList TodoList { get; set; }

		private TodoItem _addedItem;
		private int _addedIndex;

		public void Execute()
		{
			if (TodoList == null)
			{
				Console.WriteLine("Ошибка: список задач не инициализирован.");
				return;
			}

			TodoList.AddTask(Text, Flags ?? Array.Empty<string>());
			_addedIndex = TodoList.tasks.Count - 1;
			_addedItem = TodoList.tasks[_addedIndex];

			FileManager.SaveTodos(TodoList, "data/todo.csv");
		}

		public void Unexecute()
		{
			if (_addedItem != null && TodoList != null)
			{
				TodoList.tasks.RemoveAt(_addedIndex);
				Console.WriteLine("Добавление задачи отменено");
				FileManager.SaveTodos(TodoList, "data/todo.csv");
			}
		}
	}
}