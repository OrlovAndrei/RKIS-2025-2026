
namespace TodoList1;

public class ExitCommand : BaseCommand
{
	public override void Execute()
	{
		Console.WriteLine("До свидания");
		Environment.Exit(0);
	}
}