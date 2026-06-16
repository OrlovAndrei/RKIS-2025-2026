namespace TodoList;

public class AddCommand : ICommand
{
	public bool IsMultiline { get; set; }
	public string TaskText { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		if (IsMultiline)
		{
			Console.WriteLine("Введите текст задачи построчно. Завершите ввод командой !end");
			TaskText = "";
			while (true)
			{
				Console.Write("> ");
				var line = Console.ReadLine();
				if (line == "!end") break;
				TaskText += line + "\n";
			}
		}

		TodoList.Add(new TodoItem(TaskText.Trim()));
	}
}