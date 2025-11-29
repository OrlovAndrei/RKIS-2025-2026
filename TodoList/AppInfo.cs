using TodoApp.Commands;
using System.Collections.Generic;
using TodoApp;

namespace TodoApp 
{
	public static class AppInfo
	{
		public static TodoList Todos { get; set; } = new TodoList();
		public static Profile CurrentProfile { get; set; } = new Profile();

		public static Stack<BaseCommand> UndoStack { get; set; } = new Stack<BaseCommand>();
		public static Stack<BaseCommand> RedoStack { get; set; } = new Stack<BaseCommand>();
	}
}