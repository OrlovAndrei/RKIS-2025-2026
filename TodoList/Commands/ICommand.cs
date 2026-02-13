namespace TodoApp.Commands
{
	public interface ICommand
	{
		void Execute();
		Guid? CurrentProfileId { get; set; }
	}
}