using System;

namespace TodoApp.Commands
{
	public class ExitCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine("сс");
			Environment.Exit(0);
		}
	}
}