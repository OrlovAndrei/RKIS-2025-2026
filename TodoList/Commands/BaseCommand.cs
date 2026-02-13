namespace TodoApp.Commands
{
	public abstract class BaseCommand : ICommand
	{
		public Guid? CurrentProfileId { get; set; }
		public abstract void Execute();
	}
}