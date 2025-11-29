public class AddCommand : ICommand
{
	public bool Multiline { get; set; }
	public string TaskText { get; set; }
	public TodoList Todos { get; set; }
	public string TodoFilePath { get; set; }
	private int addedIndex = -1;
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
				addedIndex = Todos.Count - 1;
				Console.WriteLine("Задача добавлена");
			}
		}
		FileManager.SaveTodos(Todos, TodoFilePath);
	}
	public void Unexecute()
	{
		if (addedIndex >= 0 && addedIndex < Todos.Count)
		{
			Todos.Delete(addedIndex);
			Console.WriteLine("Добавление задачи отменено");
            FileManager.SaveTodos(Todos, TodoFilePath);
        }
    }
}