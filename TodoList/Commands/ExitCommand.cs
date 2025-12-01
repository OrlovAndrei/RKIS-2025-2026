namespace TodoList.Commands;

public class ExitCommand : ICommand
{
	public void Execute()
	{
		Environment.Exit(0);
	}
}