using System;

namespace TodoList
{

	public interface ICommand
	{
		void Execute();
	}

	public interface IUndo
	{
		void Unexecute();
	}
}
