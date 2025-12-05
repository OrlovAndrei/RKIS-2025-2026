namespace TodoApp.Commands
{
	public class ExitCommand : BaseCommand
	{
		public override void Execute()
		{
			Console.WriteLine("До свидания");
			Environment.Exit(0);
		}
		public override void Unexecute()
		{
			throw new InvalidOperationException("Невозможно отменить команду выхода из приложения");
		}
	}
}