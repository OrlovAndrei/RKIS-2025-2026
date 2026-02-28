using System;

namespace Todolist
{
	public class ExitCommand : ICommand
	{
		public TodoList TodoList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Execute()
		{
			Console.WriteLine("Выход из программы");
			Environment.Exit(0);
		}
	}
}