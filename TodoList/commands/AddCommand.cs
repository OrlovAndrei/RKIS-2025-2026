namespace TodoList.commands;

public class AddCommand : ICommand
{
	public bool IsMultiline { get; set; }
	public string TaskText { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		if (IsMultiline)
		{
			TaskText = "";
			while (true)
			{
				var line = Console.ReadLine();
				if (line == "!end") break;
				TaskText += line + "\n";
			}
		}

		TodoList.Add(new TodoItem(TaskText.Trim()));
	}
}