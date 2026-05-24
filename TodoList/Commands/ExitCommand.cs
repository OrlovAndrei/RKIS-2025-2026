namespace TodoApp.Commands
{
	public class ExitCommand : BaseCommand, ICommand
	{
		public ExitCommand() : base() { }
		public override void Execute()
		{
			Console.WriteLine("До свидания");
			Environment.Exit(0);
		}
	}
}