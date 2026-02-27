namespace TodoApp.Commands
{
	public class ExitCommand : BaseCommand, ICommand
	{
		public override void Execute()
		{
			Console.WriteLine("До свидания");
			Environment.Exit(0);
		}
	}
}