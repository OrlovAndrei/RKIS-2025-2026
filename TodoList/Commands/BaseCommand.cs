namespace TodoApp.Commands
{
	public abstract class BaseCommand
	{
		public Guid? CurrentProfileId { get; set; }
		public abstract void Execute();
		public abstract void Unexecute();
	}
}