namespace TodoList.Classes;

public class ExitCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("Программа завершена.");
		Environment.Exit(0);
	}
}