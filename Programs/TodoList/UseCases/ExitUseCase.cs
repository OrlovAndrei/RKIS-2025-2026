using TodoList.Interfaces;

namespace TodoList.UseCases;

public class ExitCommand : ICommand
{
	public async Task Execute()
	{
		Environment.Exit(0);
	}
}