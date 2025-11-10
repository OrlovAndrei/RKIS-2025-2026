namespace TodoList.Classes;

public class AddCommand : ICommand
{
	public bool IsMultiline { get; set; }
	public string TaskText { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		if (IsMultiline)
		{
			Console.WriteLine("Многострочный режим введите !q для выхода");
			TaskText = "";
			while (true)
			{
				string line = Console.ReadLine();
				if (line == "!q") break;
				TaskText += line + "\n";
			}
		}

		TodoList.Add(new TodoItem(TaskText.Trim()));
	}
}
