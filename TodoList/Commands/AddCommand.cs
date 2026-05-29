namespace TodoList.Commands;

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
			Console.WriteLine("Для выхода из многострочного режима введите !end");
			while (true)
			{
				Console.Write("> ");
				var line = Console.ReadLine();
				if (line == "!exit") break;
				TaskText = line + "\n";
			}
		}

		TodoList.Add(new TodoItem(TaskText.Trim()));
	}
}