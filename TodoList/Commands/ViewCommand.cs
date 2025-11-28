namespace TodoList.Commands
{
	public class ViewCommand : ICommand
	{
		public string[] Flags { get; set; } = Array.Empty<string>();
		public TodoList TodoList { get; set; }

		public void Execute()
		{
			TodoList.ViewTasks(Flags);
		}

		public void Unexecute() { }
	}
}