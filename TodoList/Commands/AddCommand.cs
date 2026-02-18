
namespace TodoList;
internal class AddCommand : ICommand
{
	private bool _multiline;
	public string Task { get; private set; }

	public static string CommandAddEndMark = "!end";

	public AddCommand(bool multiline, string task)
	{
		_multiline = multiline;
		Task = task;
		AppInfo.UndoPush(this);
	}

	public void Execute()
	{
		TodoItem item = new TodoItem(Task);

		if (_multiline)
		{
			Task = "";

			int lastIndex = AppInfo.CurrentTodoList.Length - 1;
			AppInfo.CurrentTodoList.Delete(lastIndex);

			for (var line = Console.ReadLine(); line != CommandAddEndMark; line = Console.ReadLine())
			{
				Task += line + "\n";
			}

			item.Text = Task;
			AppInfo.CurrentTodoList.Add(item);
		}
		else
		{
			AppInfo.CurrentTodoList.Add(item);
		}

		Console.WriteLine($"Добавлена новая задача {item.GetShortText()}");
	}

	public void Unexecute()
	{
		AppInfo.CurrentTodoList.Delete(AppInfo.CurrentTodoList.Length - 1);
		Console.WriteLine($"Добавление задачи отменено.");
	}

}
