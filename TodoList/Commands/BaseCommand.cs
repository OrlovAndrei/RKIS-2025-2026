namespace TodoApp.Commands
{
	public abstract class BaseCommand : ICommand
	{
		protected Guid? _currentProfileId;
		protected BaseCommand(Guid? currentProfileId)
		{
			_currentProfileId = currentProfileId;
			CurrentProfileId = currentProfileId;
		}
		protected BaseCommand()
		{
			_currentProfileId = null;
			CurrentProfileId = null;
		}
		public Guid? CurrentProfileId { get; set; }
		public abstract void Execute();
	}
}