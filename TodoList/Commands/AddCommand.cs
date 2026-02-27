namespace TodoApp.Commands
{
	public class AddCommand : BaseCommand, IUndo
	{
		private readonly TodoList _todoList;
		private readonly string _taskText;
		private readonly bool _multiline;
		private List<TodoItem> _addedItems = new List<TodoItem>();
		private List<int> _addedIndexes = new List<int>();

		public AddCommand(TodoList todoList, string taskText, bool multiline, Guid? currentProfileId)
		{
			_todoList = todoList;
			_taskText = taskText;
			_multiline = multiline;
			CurrentProfileId = currentProfileId;
		}

		public AddCommand(TodoList todoList, string taskText)
			: this(todoList, taskText, false, null)
		{
		}

		public override void Execute()
		{
			_addedItems.Clear();
			_addedIndexes.Clear();

			if (_multiline)
			{
				AddMultilineTask();
			}
			else
			{
				if (!string.IsNullOrEmpty(_taskText))
				{
					var item = new TodoItem(_taskText);
					_todoList.Add(item);
					_addedItems.Add(item);
					_addedIndexes.Add(_todoList.Count - 1);
					Console.WriteLine("Задача добавлена.");
				}
				else
				{
					Console.WriteLine("Ошибка: задача не может быть пустой");
				}
			}
		}

		public void Unexecute()
		{
			for (int i = _addedIndexes.Count - 1; i >= 0; i--)
			{
				if (_addedIndexes[i] < _todoList.Count)
				{
					_todoList.Delete(_addedIndexes[i]);
				}
			}
			Console.WriteLine($"Отменено добавление {_addedItems.Count} задач(и)");
		}

		private void AddMultilineTask()
		{
			Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите !end):");
			var lines = new List<string>();
			string line;

			while (true)
			{
				Console.Write("> ");
				line = Console.ReadLine();
				if (line == "!end") break;
				if (!string.IsNullOrWhiteSpace(line))
					lines.Add(line);
			}

			_addedItems.Clear();
			_addedIndexes.Clear();

			foreach (string finalTask in lines)
			{
				if (!string.IsNullOrEmpty(finalTask))
				{
					var item = new TodoItem(finalTask);
					_todoList.Add(item);
					_addedItems.Add(item);
					_addedIndexes.Add(_todoList.Count - 1);
				}
			}

			Console.WriteLine($"Добавлено {lines.Count} задач(и)");
		}
	}
}