using TodoList.classes;

namespace TodoList.commands;

public class AddCommand : ICommand
{
	public bool IsMultiline { get; init; }
	public required string TaskText { get; set; }
	public required classes.TodoList TodoList { get; init; }

	public void Execute()
	{
		if (IsMultiline)
		{
			Console.WriteLine("Многострочный режим введите !q для выхода");
			TaskText = "";
			while (true)
			{
				var line = Console.ReadLine();
				if (line == "!q") break;
				TaskText += line + "\n";
			}
		}

		TodoList.Add(new TodoItem(TaskText.Trim()));
	}
}