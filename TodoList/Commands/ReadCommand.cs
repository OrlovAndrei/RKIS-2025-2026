namespace TodoList.Commands
{
	public class ReadCommand : ICommand
	{
		public string Arg { get; set; } = string.Empty;
		public TodoList TodoList { get; set; } = null!;

		public void Execute()
		{
			TodoList.ReadTask(Arg);
		}

		public void Unexecute() { }
	}
}