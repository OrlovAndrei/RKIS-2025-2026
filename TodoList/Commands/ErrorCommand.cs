using TodoApp.Commands;

public class ErrorCommand : BaseCommand, ICommand
{
	private string _message;

	public ErrorCommand(string message)
	{
		_message = message;
	}

	public override void Execute()
	{
		Console.WriteLine($"Ошибка: {_message}");
	}
}
