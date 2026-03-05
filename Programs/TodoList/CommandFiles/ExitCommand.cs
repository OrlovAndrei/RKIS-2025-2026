using System;

namespace Todolist
{
	public class ExitCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine("Выход из программы");
			Environment.Exit(0);
		}

		public void Unexecute() 
		{
			// Выход из программы нельзя отменить
			Console.WriteLine("Нельзя отменить выход из программы");
		}

		public string Description => "Выход из программы";

	} 
}