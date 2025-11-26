public class AddCommand : ICommand
{
	public bool Multiline { get; set; }
	public string TaskText { get; set; }
	public TodoList Todos { get; set; }
	public string TodoFilePath { get; set; }
	public void Execute()
	{
		if (Multiline)
		{
			string userInput = "";
			bool isInput = true;
			string userTask = "";
			Console.WriteLine("Введите текст задачи (для завершения введите !end):");
			while (isInput)
			{
				Console.Write("> ");
				userInput = Console.ReadLine();
				if (userInput == "!end")
					isInput = false;
				else
					userTask += (userTask == "" ? "" : "\n") + userInput;
			}
			if (!string.IsNullOrEmpty(userTask))
			{
				TodoItem newTodo = new TodoItem(userTask);
				Todos.Add(newTodo);
				Console.WriteLine("Задача добавлена!");
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(TaskText))
			{
				TodoItem newTodo = new TodoItem(TaskText);
				Todos.Add(newTodo);
				Console.WriteLine("Задача добавлена");
			}
		}
	}
	public void Unexecute()
	{
		Console.WriteLine("Отмена добавления пока не реализована");
	}
}
