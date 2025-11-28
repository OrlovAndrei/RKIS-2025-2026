using System.Collections.Generic;
using TodoList.Commands;

namespace TodoList
{
	public static class AppInfo
	{
		public static TodoList Todos { get; set; }
		public static Profile CurrentProfile { get; set; }

		public static Stack<ICommand> undoStack = new Stack<ICommand>();
		public static Stack<ICommand> redoStack = new Stack<ICommand>();
	}
}