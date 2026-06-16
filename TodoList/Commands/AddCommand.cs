namespace TodoList.Commands;
public class AddCommand : ICommand
{
	public string Text { get; set; }
	public bool IsMultiTask { get; set; }
	public TodoList Todos { get; set; }

	public void Execute()
	{
		if (!IsMultiTask)
		{
			Text = ExtractTaskText(Text);
		}
		else
		{
			Console.WriteLine("Многострочный режим, введите !end для отправки");

			while (true)
			{
				string line = Console.ReadLine();
				if (line == "!end") break;
				Text += line + "\n";
			}
		}
            
		Todos.Add(new TodoItem(Text));
		Console.WriteLine($"Добавлена задача: {Todos.todos.Count}) {Text}");
	}
	static string ExtractTaskText(string input)
	{
		string[] parts = input.Split('"');
            
		if (parts.Length >= 2)
		{
			return parts[1];
		}
		else
		{
			return input.Substring(3).Trim();
		}
	}
}