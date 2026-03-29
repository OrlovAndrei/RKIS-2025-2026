namespace TodoApp.Commands
{
	public abstract class BaseCommand : ICommand
	{
		protected Guid? _currentProfileId;
		protected BaseCommand(Guid? currentProfileId)
		{
			_currentProfileId = currentProfileId;
		}
		public Guid? CurrentProfileId { get; set; }
		public abstract void Execute();
	}
}