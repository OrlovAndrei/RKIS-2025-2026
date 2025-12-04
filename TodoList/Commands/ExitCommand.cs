namespace TodoList.Commands;

public class ExitCommand : ICommand
{
	public TodoList? TodoList { get; set; }
	public Profile? UserProfile { get; set; }
	public void Execute()
	{
		Console.WriteLine("Сохранение данных...");
		
		if (UserProfile != null) FileManager.SaveProfile(UserProfile, Program.ProfileFilePath);
		if (TodoList != null) FileManager.SaveTodos(TodoList, Program.TodoFilePath);
		Environment.Exit(0);
	}
}