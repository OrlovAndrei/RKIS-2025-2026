namespace TodoList.Commands;

public class ExitCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("Выход из программы.");
		Environment.Exit(0);
	}
}