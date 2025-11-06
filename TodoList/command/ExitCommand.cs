namespace TodoApp.Commands;

public class ExitCommand : ICommand
{
	public string Name => "exit";
	public string Description => "Выйти из программы";

	public bool Execute()
	{
		Console.WriteLine("До свидания!");
		Environment.Exit(0);
		return true;
	}
}