using System;

namespace Todolist
{
	public interface ICommand
	{
		TodoList TodoList { get; set; }

		void Execute();
	}
}
