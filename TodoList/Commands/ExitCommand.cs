

namespace TodoList
{
    internal class ExitCommand : ICommand
    {
		public void Execute()
		{
			Environment.Exit(0);	
		}

		public void Unexecute() { }
	}
}
